using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Services.Helpers;

namespace MomAndBaby.API.Controllers
{
    [Route("api/helpper")]
    [ApiController]
    public class HelpperController : ControllerBase
    {
        private readonly UploadFile _uploadFile;
        public HelpperController(UploadFile uploadFile)
        {
            _uploadFile = uploadFile;
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
    }
}
