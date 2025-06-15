using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.DTO.ExpertModel;
using MomAndBaby.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace MomAndBaby.API.Controllers
{
    [Route("api/expert")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IExpertService _expertService;
        private readonly IUserPackageService _userPackageService;
        public ExpertController(IExpertService expertService, IUserPackageService userPackageService)
        {
            _expertService = expertService;
            _userPackageService = userPackageService;
        }
        [HttpGet("{expertIdOrUserId}")]
        public async Task<IActionResult> GetById(string expertIdOrUserId)
        {
            try
            {
                var result = await _expertService.GetByIdAsync(expertIdOrUserId);
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
        [HttpGet]
        public async Task<IActionResult> GetPagination(string? searchString, int pageIndex, int pageSize, bool isDescending, BaseEnum Status = BaseEnum.Active)
        {
            try
            {
                var result = await _expertService.GetPaginationAsync(searchString, pageIndex, pageSize, isDescending, Status);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateExpertModel model)
        {
            try
            {
                var result = await _expertService.UpdateExpert(id, model);
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
        [HttpGet("{expertId}/calendar")]
        public async Task<IActionResult> GetCalendarByExpertId(Guid expertId, [FromQuery] int month, [FromQuery] int year)
        {
            
            try
            {
                if (month == 0 || year == 0) throw new BaseException(StatusCodes.Status400BadRequest, "Month and year must be provided");
                var result = await _userPackageService.GetCalendarExpertByExpertId(expertId, month, year);
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
