using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Util.Communication
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Timeout { get; set; }
        public string SMTP { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Salt { get; set; }
        public string ClientURL { get; set; }
        public int ResetTimeout { get; set; }
        public int PasswordExpiration { get; set; }
    }
}
