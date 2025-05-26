using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Services.DTO.AppointmentModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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
    }
}
