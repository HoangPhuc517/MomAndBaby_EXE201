using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IExpertService _expertService;
        public AdminController(IExpertService expertService)
        {
            _expertService = expertService;
        }

        [HttpPut("expert/{id}")]
        public async Task<IActionResult> UpdateExpert(string id, BaseEnum status)
        {
            try
            {
                var result = await _expertService.UpdateStatusExpert(id, status);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
