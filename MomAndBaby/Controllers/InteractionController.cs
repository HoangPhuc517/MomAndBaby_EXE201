using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/interaction")]
    [ApiController]
    public class InteractionController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public InteractionController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost("like/{blogId}")]
        public async Task<IActionResult> LikeBlog(Guid blogId)
        {
            try
            {
                var result = await _blogService.CreateLikeBlog(blogId);
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

        [HttpPost("comment/{blogId}")]
        public async Task<IActionResult> CommentBlog(Guid blogId, [FromBody] string comment)
        {
            try
            {
                var result = await _blogService.CreateCommentBlog(blogId, comment);
                return Ok(new { result.Item1, result.Item2 });
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

        [HttpDelete("comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            try
            {
                var result = await _blogService.DeleteCommentBlog(commentId);
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

        [HttpDelete("like/{blogId}")]
        public async Task<IActionResult> UnlikeBlog(Guid blogId)
        {
            try
            {
                var result = await _blogService.DeleteLikeBlog(blogId);
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
