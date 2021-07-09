using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class InsuranceInfoDTO
    {
        public int ID { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        [Range(0, 9999999)]
        public decimal? BasicSalary { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }


        public IEnumerable<InsuranceInfoDetailDTO> InsuranceInfoDetail { get; set; }



    }

    public class SaveInsuranceInfoDTO
    {
        public int ID { get; set; }
        public int? ID_InsuranceInfo { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        [Range(0, 9999999)]
        public decimal? BasicSalary { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
    }

    public class InsurancePageRequest
    {
        public int Page { get; set; }
        public int Pagesize { get; set; }


        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BasicSalaryStart { get; set; }
        public decimal? BasicSalaryEnd { get; set; }

    }

    public class InsuranceCountRequest
    {
        
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BasicSalaryStart { get; set; }
        public decimal? BasicSalaryEnd { get; set; }

    }

}
