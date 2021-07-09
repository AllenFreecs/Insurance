using System;
using System.Collections.Generic;

namespace Insurance.DL.Entities
{
    public partial class InsuranceInfoDetail
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public int? ID_InsuranceInfo { get; set; }
        public int? Multiple { get; set; }
        public decimal? BenefitsAmountQuotation { get; set; }
        public decimal? PendedAmount { get; set; }
        public string Benefits { get; set; }
    }
}
