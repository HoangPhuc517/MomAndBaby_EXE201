using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.DTO.ChatModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpPost("hub")]
        public async Task<IActionResult> CreateChatHub(Guid secondUserId, string nameChatHub)
        {
            try
            {
                var result = await _chatService.CreateChatHup(secondUserId, nameChatHub);
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
        [HttpGet("hub/{id}")]
        public async Task<IActionResult> GetChatHubById(Guid id)
        {
            try
            {
                var result = await _chatService.GetChatHupById(id);
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

        [HttpGet("hubs/user/{id}")]
        public async Task<IActionResult> GetChatHubs(Guid id)
        {
            try
            {
                var result = await _chatService.GetAllChatHupsByUserId(id);
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

        [HttpPut("hub/{id}")]
        public async Task<IActionResult> UpdateChatHub(Guid id, [FromBody] string nameChatHub)
        {
            try
            {
                var result = await _chatService.UpdateChatHup(id, nameChatHub);
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

        [HttpPost("message")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageModel model)
        {
            try
            {
                var type = model.Type.ToString();
                await _chatService.CreateChatMessage(model.ChatHubId, model.Content, type);
                return Ok("Saved successfull.");
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
        [HttpDelete("message/{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            try
            {
                await _chatService.DeleteChatMessage(id);
                return Ok("Deleted successfull.");
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

        [HttpPut("message/{id}")]
        public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] string content)
        {
            try
            {
                await _chatService.UpdateChatMessage(id, content);
                return Ok("Updated successfull.");
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
