using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class SetupDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal? GuaranteedIssue { get; set; }
        [Required]
        public int? MaxAgeLimit { get; set; }
        [Required]
        public int? MinAgeLimit { get; set; }
        [Required]
        public int? MinimumRange { get; set; }
        [Required]
        public int? MaximumRange { get; set; }
        [Required]
        public int? Increments { get; set; }

    }
}
