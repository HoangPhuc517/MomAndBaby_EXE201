using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Services.DTO.AppointmentModel;
using MomAndBaby.Services.DTO.FeedbackModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IFeedbackService _feedbackService;

        public AppointmentController(IAppointmentService appointmentService, IFeedbackService feedbackService)
        {
            _appointmentService = appointmentService;
            _feedbackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO appointment)
        {
            try
            {
                var result = await _appointmentService.CreateAppointment(appointment);
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
        public async Task<IActionResult> UpdateStatusAppointment(string id, [FromBody] AppointmentStatusEnum statusEnum)
        {
            try
            {
                await _appointmentService.UpdateStatusAppointment(id, statusEnum);
                return Ok();
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(string id)
        {
            try
            {
                var result = await _appointmentService.GetAppointmentById(id);
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

        [HttpGet("pagination")]
        public async Task<IActionResult> GetAppointmentByPagination(int pageIndex, int pageSize, string? searchString, string? userId, AppointmentStatusEnum? appointmentStarusEnum, AppointmentTypeEnum? appointmentTypeEnum, bool isDescending)
        {
            try
            {
                var result = await _appointmentService.GetAppointmentByPagination(pageIndex, pageSize, searchString, userId, appointmentStarusEnum, appointmentTypeEnum, isDescending);
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

        [HttpPost("feedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDTO feedback)
        {
            try
            {
                var result = await _feedbackService.CreateFeeback(feedback);
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

        [HttpGet("feedback/{feedbackId}")]
        public async Task<IActionResult> GetFeedbackById(Guid feedbackId)
        {
            try
            {
                var result = await _feedbackService.GetFeedbackById(feedbackId);
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

        [HttpGet("feedback/expert/{id}")]
        public async Task<IActionResult> GetFeedbackOfExpert(int pageSize, int pageIndex, bool isDescending, Guid id)
        {
            try
            {
                var result = await _feedbackService.GetFeedbackOfExpert(pageSize, pageIndex, isDescending, id);
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

        [HttpPut("feedback")]
        public async Task<IActionResult> UpdateFeedback([FromBody] UpdateFeedbackDTO updateModel)
        {
            try
            {
                var result = await _feedbackService.UpdateFeeback(updateModel);
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
