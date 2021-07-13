using System;
using System.Collections.Generic;

namespace Insurance.DL.Entities
{
    public partial class InsuranceInfo
    {
        public int ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public decimal? BasicSalary { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
