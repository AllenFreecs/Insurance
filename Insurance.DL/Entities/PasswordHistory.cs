using System;
using System.Collections.Generic;

namespace Insurance.DL.Entities
{
    public partial class PasswordHistory
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public int? ID_User { get; set; }
        public string Password { get; set; }
    }
}
