using System;
using System.Collections.Generic;

namespace Insurance.DL.Entities
{
    public partial class UserGroup
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
    }
}
