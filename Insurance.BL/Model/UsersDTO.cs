using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Model
{
    public class UsersDTO
    {
        public int ID { get; set; }
        public bool IsConfirmed { get; set; }
        public int? ID_UserGroup { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public string GUID { get; set; }
        public string Token { get; set; }
        public int? InvalidAttempts { get; set; }
    }
}
