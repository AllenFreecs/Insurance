using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class InsuranceInfoDetailDTO
    {
        public int ID { get; set; }
        public int? ID_InsuranceInfo { get; set; }
        public int? Multiple { get; set; }
        public decimal? BenefitsAmountQuotation { get; set; }
        public decimal? PendedAmount { get; set; }
        public string Benefits { get; set; }

    }
}
