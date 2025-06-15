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
        private readonly IPackageService _packageService;
        private readonly IDealService _dealService;
        private readonly IFeedbackService _feedbackService;
        private readonly IUserPackageService _userPackageService;
        public AdminController(IExpertService expertService, IPackageService packageService, IDealService dealService, IFeedbackService feedbackService, IUserPackageService userPackageService)
        {
            _expertService = expertService;
            _packageService = packageService;
            _dealService = dealService;
            _feedbackService = feedbackService;
            _userPackageService = userPackageService;
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

        [HttpPut("package/{id}")]
        public async Task<IActionResult> UpdatePackage(string id, BaseEnum status)
        {
            try
            {
                await _packageService.UpdateStatusPackage(id, status);
                return Ok("Successfull.");
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

        [HttpPut("deal/{id}")]
        public async Task<IActionResult> UpdateDeal(string id, BaseEnum status)
        {
            try
            {
                await _dealService.UpdateStatusDeal(id, status);
                return Ok("Successfull.");
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetFeedbacks()
        {
            try
            {
                var result = await _feedbackService.GetAllFeedback();


                return Ok(new
                {
                    feedbacks = result.Item1,
                    feedbackCount = result.Item2,
                    userPayPackageCount = await _userPackageService.UserPackageTotalCount(),
                });
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
