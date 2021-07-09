using AutoMapper;
using Insurance.BL.Auth;
using Insurance.BL.Model;
using Insurance.BL.Util.Communication;
using Insurance.BL.Util.Encryption;
using Insurance.BL.Util.Settings;
using Insurance.DL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Insurance.BL.Auth
{
    public class UsersBL : IUsersBL
    {
        private InsuranceDBContext _dbcontext;
        private MailSender _mailSender;
        private AppSettings appSettings;

        public UsersBL(InsuranceDBContext SpeedometerAPIContext, MailSender mailSender, SettingsManager settings)
        {
            _dbcontext = SpeedometerAPIContext;
            _mailSender = mailSender;
            appSettings = settings.LoadSettings();
        }
        public async Task<GlobalResponseDTO> Authenticate(string username, string password, HttpResponse response)
        {
            try
            {

                string Encpassword = RIJEncrypt.Encrypt(password, appSettings.Salt);

                var user = await _dbcontext.Users.SingleOrDefaultAsync(x => x.UserName == username && x.Password == Encpassword && x.IsActive == true && x.IsConfirmed == true);
                // return null if user not found
                if (user == null)
                {
                    var currentuser = await _dbcontext.Users.SingleOrDefaultAsync(x => x.UserName == username && x.IsActive == true && x.IsConfirmed == true);

                    //Check attempts
                    int? numattempts = currentuser.InvalidAttempts;
                    if (numattempts == 3)
                    {

                        return new GlobalResponseDTO { IsSuccess = false, Message = "Account is locked." };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currentuser.UserName))
                        {
                            using (var transaction = _dbcontext.Database.BeginTransaction())
                            {
                                try
                                {
                                    currentuser.InvalidAttempts = currentuser.InvalidAttempts.GetValueOrDefault() + 1;
                                    _dbcontext.Entry(currentuser).State = EntityState.Modified;
                                    _dbcontext.SaveChanges();
                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();
                                    throw;
                                }
                            }
                        }

                        return new GlobalResponseDTO { IsSuccess = false, Message = "Username or password is incorrect." };
                    }


                }

                // create token
                var Token = ReIssuetoken(user.ID.ToString(), user.ID_UserGroup.ToString(), response);
                user.Token = Token;

                using (var transaction = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbcontext.Entry(user).State = EntityState.Modified;
                        _dbcontext.SaveChanges();
                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }


                return new GlobalResponseDTO { IsSuccess = true, Message = "Authenticated." }; ;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> ConfirmRegistration(string guid)
        {
            try
            {
                //Check guid authenticity
                string decguid = RIJEncrypt.Decrypt(guid, appSettings.Salt);
                int id = Convert.ToInt32(decguid.Substring(0, decguid.IndexOf('-')));
                DateTime resetDate = Convert.ToDateTime(decguid.Substring(decguid.LastIndexOf('-') + 1));

                if ((DateTime.UtcNow - resetDate).TotalMinutes > appSettings.ResetTimeout)
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Request is expired" };
                }
                else
                {


                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {
                        try
                        {
                            var data = await _dbcontext.Users.Where(c => c.ID == id).SingleOrDefaultAsync();
                            data.IsConfirmed = true;

                            _dbcontext.Entry(data).State = EntityState.Modified;
                            await _dbcontext.SaveChangesAsync();
                            transaction.Commit();

                            return new GlobalResponseDTO() { IsSuccess = true, Message = "Email Confirmed" };
                        }
                        catch
                        {
                            transaction.Rollback();
                            return new GlobalResponseDTO() { IsSuccess = false, Message = "Server processes error" };
                            throw;
                        }


                    }




                }


            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> CreateUser(UserCreationDTO userCreationDTO)
        {
            try
            {
                //Check if theres an existing Username

                if (_dbcontext.Users.Where(c => c.UserName == userCreationDTO.UserName).Any())
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Username must be unique" };
                }
                if (_dbcontext.Users.Where(c => c.Email == userCreationDTO.Email).Any())
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Email already in use." };
                }

                string input = userCreationDTO.Password;
                string password = RIJEncrypt.Encrypt(input, appSettings.Salt);

                var hasSymbols = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");

                if (!hasSymbols.IsMatch(input))
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Password not acceptable" };
                }

                using (var transaction = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        //User
                        var data = Mapper.Map<UserCreationDTO, Users>(userCreationDTO);
                        data.Password = password;
                        data.PasswordExpirationDate = DateTime.Now.AddDays(appSettings.PasswordExpiration);

                        _dbcontext.Entry(data).State = EntityState.Added;
                        await _dbcontext.SaveChangesAsync();
                        transaction.Commit();

                        //History
                        await AddHistory(data.ID, data.Password);

                        //Send confirmation email
                        string key = string.Concat(data.ID, "-", DateTime.UtcNow);
                        string encid = RIJEncrypt.Encrypt(key, appSettings.Salt);

                        string url = Path.Combine(appSettings.ClientURL, encid);

                        var htmlTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Util", "Template", "confirmation.html");


                        htmlTemplate = File.ReadAllText(htmlTemplate);
                        htmlTemplate = htmlTemplate.Replace("{link}", url).Replace("{UserName}", data.UserName);

                        EmailModel email = new EmailModel();
                        email.From = appSettings.Email;
                        email.Recipients = data.Email;
                        email.Body = htmlTemplate;
                        email.Subject = "Your QLess Confirmation Link";

                        await _mailSender.Send(email);


                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new GlobalResponseDTO() { IsSuccess = false, Message = "Server processes error" };
                        throw;
                    }


                }




                return new GlobalResponseDTO() { IsSuccess = true, Message = "User was created" };


            }
            catch (Exception ex)
            {

                return new GlobalResponseDTO() { IsSuccess = false, Message = "Server processes error" };
            }
        }

        public async Task<bool> ForgeryDetected(string token, int userID)
        {
            try
            {
                token = token.Replace("Bearer ", string.Empty);

                var user = await _dbcontext.Users.SingleOrDefaultAsync(x => x.ID == userID);

                if (user.Token != token)
                {
                    //Forgery Detected
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> ForgotPassword(string Username)
        {
            try
            {

                bool existing = _dbcontext.Users.Where(c => c.UserName == Username).Any();
                string key;
                string encId;

                if (existing)
                {

                    var user = _dbcontext.Users.Where(c => c.UserName == Username).SingleOrDefault();
                    key = string.Concat(user.ID, "-", DateTime.UtcNow);
                    encId = RIJEncrypt.Encrypt(key, appSettings.Salt);

                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {
                        try
                        {

                            user.GUID = encId;
                            _dbcontext.Entry(user).State = EntityState.Modified;
                            await _dbcontext.SaveChangesAsync();
                            transaction.Commit();

                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }



                    string url = Path.Combine(appSettings.ClientURL, encId);

                    var htmlTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Util", "Template", "forgotpassword.html");


                    htmlTemplate = File.ReadAllText(htmlTemplate);
                    htmlTemplate = htmlTemplate.Replace("{link}", url);

                    EmailModel email = new EmailModel();
                    email.From = appSettings.Email;
                    email.Recipients = user.Email;
                    email.Body = htmlTemplate;
                    email.Subject = "Your QLess Reset Link";



                    return await _mailSender.Send(email);
                }
                else
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Username not exist" };
                }


            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> ForgotUser(string email)
        {
            try
            {
                bool existing = _dbcontext.Users.Where(c => c.Email == email).Any();

                if (existing)
                {
                    string Username = _dbcontext.Users.Where(c => c.Email == email).Select(c => c.UserName).SingleOrDefault().ToString();



                    var htmlTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Util", "Template", "forgotusername.html");


                    htmlTemplate = File.ReadAllText(htmlTemplate);
                    htmlTemplate = htmlTemplate.Replace("{UserName}", Username);

                    EmailModel emailmodel = new EmailModel();
                    emailmodel.From = appSettings.Email;
                    emailmodel.Recipients = email;
                    emailmodel.Body = htmlTemplate;
                    emailmodel.Subject = "Your QLess Username";

                    return await _mailSender.Send(emailmodel);
                }
                else
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Username not exist" };
                }


            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public string ReIssuetoken(string claimID, string roleID, HttpResponse response)
        {
            try
            {
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, claimID),
                     new Claim(ClaimTypes.Role, roleID),
                    }),
                    Issuer = appSettings.Issuer,
                    Audience = appSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(appSettings.Timeout),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var newtoken = tokenHandler.WriteToken(token);

                // issue client token
                response.Cookies.Append("Auth", newtoken, new CookieOptions { Path = "/", HttpOnly = false });

                return newtoken;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> ResetPassword(string guid, string password)
        {
            try
            {
                //Check guid authenticity
                string decguid = RIJEncrypt.Decrypt(guid, appSettings.Salt);
                string oldpassword;

                int id = Convert.ToInt32(decguid.Substring(0, decguid.IndexOf('-')));
                DateTime resetDate = Convert.ToDateTime(decguid.Substring(decguid.LastIndexOf('-') + 1));

                if ((DateTime.UtcNow - resetDate).TotalMinutes > appSettings.ResetTimeout)
                {
                    return new GlobalResponseDTO() { IsSuccess = false, Message = "Reset token is expired" };
                }
                else
                {
                    //Encrypt password
                    password = RIJEncrypt.Encrypt(password, appSettings.Salt);

                    //PasswordChecker
                    var historicaldata = _dbcontext.PasswordHistory.Where(c => c.ID_User == id && c.Password == password).Take(3).OrderByDescending(c => c.ID).Count();

                    //User Data
                    var data = _dbcontext.Users.Where(c => c.ID == id).SingleOrDefault();
                    oldpassword = data.Password;


                    if (historicaldata == 0 && data.GUID != null)
                    {
                        using (var transaction = _dbcontext.Database.BeginTransaction())
                        {
                            try
                            {

                                data.Password = password;
                                data.GUID = null;
                                var history = new PasswordHistory();
                                history.Password = oldpassword;
                                history.ID_User = id;

                                _dbcontext.Entry(history).State = EntityState.Added;
                                _dbcontext.Entry(data).State = EntityState.Modified;
                                await _dbcontext.SaveChangesAsync();
                                transaction.Commit();

                            }
                            catch
                            {
                                transaction.Rollback();
                                return new GlobalResponseDTO() { IsSuccess = false, Message = "Server processes error" };
                                throw;
                            }

                            return new GlobalResponseDTO() { IsSuccess = true, Message = "Password was reset." };
                        }
                    }

                    else
                    {
                        return new GlobalResponseDTO() { IsSuccess = true, Message = "Password cannot be re-used." };
                    }



                }


            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }
        public async Task AddHistory(int id, string password)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {

                    //History
                    var history = new PasswordHistory();
                    history.ID_User = id;
                    history.Password = password;
                    _dbcontext.Entry(history).State = EntityState.Added;
                    await _dbcontext.SaveChangesAsync();
                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<GlobalResponseDTO> HeartBeat(string claimID, string roleID, HttpResponse response)
        {
            try
            {
                // create token
                var Token = ReIssuetoken(claimID, roleID, response);
                var user = await _dbcontext.Users.SingleOrDefaultAsync(x => x.ID == Convert.ToInt32(claimID));
                user.Token = Token;

                using (var transaction = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbcontext.Entry(user).State = EntityState.Modified;
                        _dbcontext.SaveChanges();
                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return new GlobalResponseDTO { IsSuccess = true };

            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }
    }
}
