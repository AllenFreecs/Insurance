using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.BL.Model;
using Insurance.Insurance.BL.insurancedetail;
using Insurance.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceDetailController : ControllerBase
    {
        private readonly IInsuranceDetailBL _InsuranceDetailbl;
        public InsuranceDetailController(IInsuranceDetailBL InsuranceDetailBL)
        {
            _InsuranceDetailbl = InsuranceDetailBL;
        }

        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<InsuranceInfoDetailDTO>), 200)]
        public async Task<IActionResult> GetInsuranceDetailList(PageRequest paging)
        {
            try
            {
                var data = await _InsuranceDetailbl.GetInsuranceDetailList(paging);

                if (data == null)
                {
                    return NotFound();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("data")]
        [ProducesResponseType(typeof(InsuranceInfoDetailDTO), 200)]
        public async Task<IActionResult> GetInsuranceDetailData(int ID)
        {
            try
            {
                var data = await _InsuranceDetailbl.GetInsuranceDetailData(ID);

                if (data == null)
                {
                    return NotFound();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public async Task<IActionResult> DeleteInsuranceDetailData(IEnumerable<int> IDs)
        {
            try
            {
                var data = await _InsuranceDetailbl.DeleteInsuranceDetail(IDs);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("save")]
        [ProducesResponseType(typeof(GlobalResponseDTO), 200)]
        public async Task<IActionResult> SaveInsuranceDetailData(InsuranceInfoDetailDTO model)
        {
            try
            {
                var data = await _InsuranceDetailbl.SaveInsuranceDetail(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}