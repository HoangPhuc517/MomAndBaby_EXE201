using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.DTO.UserPackageModel;
using MomAndBaby.Services.DTO.VnPayModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IUserPackageService _userPackageService;
        private readonly ITransactionService _transactionService;


        public PaymentController(IVnPayService vnPayService, IUserPackageService userPackageService, ITransactionService transactionService)
        {
            _vnPayService = vnPayService;
            _userPackageService = userPackageService;
            _transactionService = transactionService;
        }

        [HttpPost("vnpay/service-package")]
        public async Task<IActionResult> CreatePaymentServicePackage(CreateUserPackage model)
        {
            try
            {
                var userPackage = await _userPackageService.CreateUserPackage(model);
                if (userPackage.Amount == 0)
                {
                    return Ok("Payment success");
                }
                var payload = new VnPayRequestModel
                {
                    Id = userPackage.Id.ToString(),
                    Amount = userPackage.Amount,
                    UserId = userPackage.UserId.ToString(),
                    CreatedDate = userPackage.CreatedTime.DateTime
                };
                var url = _vnPayService.CreateVNPayUrl(HttpContext, payload);
                return Ok(url);
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

        [HttpGet("vnpay/callback")]
        public async Task<IActionResult> VnPayCallback()
        {
            try
            {
                var createTransactionDTO = _vnPayService.ProcessVNPayResponse(Request.Query);
                await _transactionService.CreateTransaction(createTransactionDTO);
                return Ok(true);
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

        [HttpGet("transaction/fillter")]
        public async Task<IActionResult> GetTransactionByPaging(int pageIndex, int pageSize, string? stringSearch, DateTime? startDate, DateTime? endDate, bool isDescending, string? userId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionByPaging(pageIndex, pageSize, stringSearch, startDate, endDate, isDescending, userId);
                return Ok(transactions);
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

        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransactionById(string id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionById(id);
                return Ok(transaction);
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

        [HttpGet("transaction/dashboard")]
        public async Task<IActionResult> GetTransactionDashboard(int month, int year, string? userId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionByMonth(month, year, userId);
                decimal total = 0;
                if (transactions.Count != 0)
                {
                    foreach (var i in transactions)
                    {
                        total += i.Amount;
                    }
                }
                return Ok(new
                {
                    transactions,
                    total = total
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
