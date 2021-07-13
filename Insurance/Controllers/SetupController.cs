using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.BL.Model;
using Insurance.BL.Util;
using Insurance.Insurance.BL.setup;
using Insurance.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Insurance.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ISetupBL _Setupbl;
        public SetupController(ISetupBL SetupBL)
        {
            _Setupbl = SetupBL;
        }

        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<SetupDTO>), 200)]
        public async Task<IActionResult> GetSetupList(PageRequest paging)
        {
            try
            {
                var data = await _Setupbl.GetSetupList(paging);

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
        [ProducesResponseType(typeof(SetupDTO), 200)]
        public async Task<IActionResult> GetSetupData(int ID)
        {
            try
            {
                var data = await _Setupbl.GetSetupData(ID);

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
        public async Task<IActionResult> DeleteSetupData(IEnumerable<int> IDs)
        {
            try
            {
                var data = await _Setupbl.DeleteSetup(IDs);

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
        public async Task<IActionResult> SaveSetupData(SetupDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var data = await _Setupbl.SaveSetup(model);
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
    }
}