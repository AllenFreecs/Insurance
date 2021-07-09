using Insurance.BL.Model;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Insurance.BL.setup
{
    public interface ISetupBL
    {
        Task<IEnumerable<SetupDTO>> GetSetupList(PageRequest paging);
        Task<SetupDTO> GetSetupData(int ID);
        Task<GlobalResponseDTO> SaveSetup(SetupDTO model);
        Task<GlobalResponseDTO> DeleteSetup(IEnumerable<int> IDs);
    }
}
