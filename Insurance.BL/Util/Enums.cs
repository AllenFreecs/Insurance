using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Util
{
    public enum Roads : int
    {
        National = 1,
        Street = 2,
        Expressway = 3
    }

    public sealed class UserRole
    {
        public const string Admin = "1";
        public const string User = "2";

    }


}
