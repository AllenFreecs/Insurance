using Insurance.BL.Model;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Insurance.BL.insurancedetail
{
    public interface IInsuranceDetailBL
    {
        Task<IEnumerable<InsuranceInfoDetailDTO>> GetInsuranceDetailList(PageRequest paging);
        Task<InsuranceInfoDetailDTO> GetInsuranceDetailData(int ID);
        Task<GlobalResponseDTO> SaveInsuranceDetail(InsuranceInfoDetailDTO model);
        Task<GlobalResponseDTO> DeleteInsuranceDetail(IEnumerable<int> IDs);
    }
}
