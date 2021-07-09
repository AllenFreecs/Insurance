using Insurance.BL.Auth;
using Insurance.BL.Model;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpeedometerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUsersBL _userBL;
        private IAntiforgery _antiForgery;

        public AuthController(IUsersBL userBL, IAntiforgery antiForgery)
        {
            _userBL = userBL;
            _antiForgery = antiForgery;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public async Task<IActionResult> Authenticate(string UserName, string Password)
        {
            try
            {
                var user = await _userBL.Authenticate(UserName, Password, Response);

                if (user.IsSuccess)
                {
                    var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
                    Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
                    {
                        HttpOnly = true
                    });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet, Route("logout")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Auth");
            Response.Cookies.Delete("XSRF-TOKEN");
            Response.Cookies.Delete("CXSRF");
            return Ok("LOGGED_OUT");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public IActionResult ForgotPassword(string UserName)
        {
            try
            {
                var user = _userBL.ForgotPassword(UserName);

                return Ok(user.Result);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotusername")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public IActionResult ForgotUsername(string Email)
        {
            try
            {
                var user = _userBL.ForgotUser(Email);

                return Ok(user.Result);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("reset")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public async Task<IActionResult> ResetPassword(string guid, string password)
        {
            try
            {
                var user = await _userBL.ResetPassword(guid, password);

                return Ok(user);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("confirm")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public async Task<IActionResult> ConfirmAccount(string guid)
        {
            try
            {
                var user = await _userBL.ConfirmRegistration(guid);

                return Ok(user);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("heartbeat")]
        public async Task<IActionResult> HeartBeat()
        {
            try
            {
                if (!String.IsNullOrEmpty(User.Identity.Name))
                {
                    var accesToken = Request.Headers["Authorization"];
                    string claimid = User.FindFirstValue(ClaimTypes.Name);
                    string roleid = User.FindFirstValue(ClaimTypes.Role);
                    bool Forged = await _userBL.ForgeryDetected(accesToken, Convert.ToInt32(claimid));
                    if (!Forged)
                    {
                        //_userBL.ReIssuetoken(claimid, roleid, Response);
                        var resp = await _userBL.HeartBeat(claimid, roleid, Response);

                        if (resp.IsSuccess)
                        {
                            return Ok(new { message = "Authenticated" });
                        }
                        else {
                            return Ok(new { message = "Issue granting token" });
                        }
                        
                    }
                    else
                    {
                        return BadRequest(new { message = "Invalid token" });
                    }
                }
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }



        }


        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = UserRole.Admin)]
        [Route("CreateUser")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public async Task<IActionResult> CreateUser(UserCreationDTO userCreationDTO)
        {
            try
            {
                var user = await _userBL.CreateUser(userCreationDTO);

                return Ok(user);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }

        }
    }
}
