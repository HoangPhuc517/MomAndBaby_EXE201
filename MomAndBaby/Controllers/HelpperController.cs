using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.DTO.AppointmentModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/helpper")]
    [ApiController]
    public class HelpperController : ControllerBase
    {
        private readonly UploadFile _uploadFile;
        private readonly IAppointmentService _appointmentService;
        public HelpperController(UploadFile uploadFile, IAppointmentService appointmentService)
        {
            _uploadFile = uploadFile;
            _appointmentService = appointmentService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                var url = await _uploadFile.UploadImageAsync(stream, file.FileName);
                return Ok(url);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("meet-link")]
        public IActionResult GenerateMeetingLink(string topic = "Meeting")
        {
            // Tạo room name ngẫu nhiên
            var roomId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            var roomName = $"{topic}-{roomId}";
            var meetingUrl = $"https://meet.jit.si/{roomName}";

            return Ok(new
            {
                topic,
                meetingUrl
            });
        }

        [HttpPost("timeslot")]
        public async Task<IActionResult> CreateTimeSlot(CreateTimeSlotDTO model)
        {
            try
            {
                string convertTime = model.StartTime + "-" + model.EndTime;
                var (StartTime, EndTime) = TimeOnlyProccess.ConvertToTimeOnlyRange(convertTime);
                var result = await _appointmentService.CreateTimeSlot(StartTime, EndTime);
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

        [HttpGet("timeslot")]
        public async Task<IActionResult> GetTimeSlot()
        {
            try
            {
                var result = await _appointmentService.GetTimeSlots();
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
