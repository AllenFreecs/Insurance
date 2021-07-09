using Microsoft.Extensions.Configuration;
using Insurance.BL.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Insurance.BL.Util.Settings;
using System.Threading.Tasks;

namespace Insurance.BL.Util.Communication
{
    public class MailSender
    {
        private AppSettings appSettings = new AppSettings();
        public MailSender(IConfiguration configuration, SettingsManager settings)
        {
            appSettings = settings.LoadSettings();
        }
        public async Task<GlobalResponseDTO> Send(EmailModel email)
        {
            try
            {
                var client = new SmtpClient(appSettings.SMTP, appSettings.Port)
                {
                    Credentials = new NetworkCredential(appSettings.Email, appSettings.Password),
                    EnableSsl = true,

                };

                //client.Send(email.From, email.Recipients, email.Subject, email.Body);
                MailMessage mail = new MailMessage(email.From, email.Recipients, email.Subject, email.Body);
                mail.IsBodyHtml = true;

                client.Send(mail);

                return new GlobalResponseDTO() { IsSuccess = true, Message = "Email sent." };



            }
            catch (Exception)
            {
                return new GlobalResponseDTO() { IsSuccess = false, Message = "Server error" };
            }
        }
    }
}
