using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.DTO.JournalModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/journal")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        public readonly IJournalService _journalService;
        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(JournalDto journalDto)
        {
            try
            {
                var result = await _journalService.CreateJournal(journalDto);
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

        [HttpPut("{journalId}")]
        public async Task<IActionResult> UpdateJournal(string journalId, JournalDto journalDto)
        {
            try
            {
                var result = await _journalService.UpdateJournal(journalId, journalDto);
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

        [HttpDelete("{journalId}")]
        public async Task<IActionResult> DeleteJournal(string journalId)
        {
            try
            {
                await _journalService.DeleteJournal(journalId);
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

        [HttpGet]
        public async Task<IActionResult> GetJournal(
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] string? headOrContent,
            [FromQuery] bool isDescending)
        {
            try
            {
                var result = await _journalService.GetJournals(pageIndex, pageSize, headOrContent, isDescending);
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

        [HttpGet("period")]
        public async Task<IActionResult> GetJournalsByPeriod(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var result = await _journalService.GetJournalsByPeriod(startDate, endDate);
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
