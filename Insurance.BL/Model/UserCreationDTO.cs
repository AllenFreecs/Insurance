using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Model
{
    public class UserCreationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int ID_UserGroup { get; set; }
    }
}
