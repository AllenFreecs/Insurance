using Insurance.BL.Model;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Insurance.BL.insurance
{
    public interface IInsuranceBL
    {
        Task<IEnumerable<InsuranceInfoDTO>> GetInsuranceList(InsurancePageRequest paging);
        Task<InsuranceInfoDTO> GetInsuranceData(int ID);
        Task<GlobalResponseDTO> SaveInsurance(InsuranceInfoDTO model);
        Task<GlobalResponseDTO> DeleteInsurance(IEnumerable<int> IDs);
        Task<int?> GetInsuranceCount(InsuranceCountRequest model);
    }
}
