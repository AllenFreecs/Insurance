using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class SetupDTO
    {
        public int Id { get; set; }
        public decimal? GuaranteedIssue { get; set; }
        public int? MaxAgeLimit { get; set; }
        public int? MinAgeLimit { get; set; }
        public int? MinimumRange { get; set; }
        public int? MaximumRange { get; set; }
        public int? Increments { get; set; }

    }
}
