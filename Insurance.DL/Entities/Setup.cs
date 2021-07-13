using System;
using System.Collections.Generic;

namespace Insurance.DL.Entities
{
    public partial class Setup
    {
        public int ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public decimal? GuaranteedIssue { get; set; }
        public int? MaxAgeLimit { get; set; }
        public int? MinAgeLimit { get; set; }
        public int? MinimumRange { get; set; }
        public int? MaximumRange { get; set; }
        public int? Increments { get; set; }
    }
}
