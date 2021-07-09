using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.BL.Model;
using Insurance.Insurance.BL.insurance;
using Insurance.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceBL _Insurancebl;
        public InsuranceController(IInsuranceBL InsuranceBL)
        {
            _Insurancebl = InsuranceBL;
        }

        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<InsuranceInfoDTO>), 200)]
        public async Task<IActionResult> GetInsuranceList(InsurancePageRequest paging)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var data = await _Insurancebl.GetInsuranceList(paging);
                    if (data == null)
                    {
                        return NotFound();
                    }

                    return Ok(data);
                }
                else {
                    string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                    throw new Exception(messages);
                }
                    

              
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("data")]
        [ProducesResponseType(typeof(InsuranceInfoDTO), 200)]
        public async Task<IActionResult> GetInsuranceData(int ID)
        {
            try
            {
                var data = await _Insurancebl.GetInsuranceData(ID);

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
        public async Task<IActionResult> DeleteInsuranceData(IEnumerable<int> IDs)
        {
            try
            {
                var data = await _Insurancebl.DeleteInsurance(IDs);

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
        public async Task<IActionResult> SaveInsuranceData(InsuranceInfoDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var data = await _Insurancebl.SaveInsurance(model);
                    if (data == null)
                    {
                        return NotFound();
                    }

                    return Ok(data);
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                    throw new Exception(messages);
                }


            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int?), 200)]
        public async Task<IActionResult> GetInsuranceCount(InsuranceCountRequest model)
        {
            try
            {
                var data = await _Insurancebl.GetInsuranceCount(model);

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
    }
}