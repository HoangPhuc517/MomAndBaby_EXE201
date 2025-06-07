using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.DTO.UserPackageModel;
using MomAndBaby.Services.DTO.VnPayModel;
using MomAndBaby.Services.Interface;
using Net.payOS.Types;

namespace MomAndBaby.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IUserPackageService _userPackageService;
        private readonly ITransactionService _transactionService;
        private readonly IPayOsService _payOsService;


        public PaymentController(IVnPayService vnPayService, IUserPackageService userPackageService, ITransactionService transactionService, IPayOsService payOsService)
        {
            _vnPayService = vnPayService;
            _userPackageService = userPackageService;
            _transactionService = transactionService;
            _payOsService = payOsService;
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
                decimal _total = 0;
                if (transactions.Count != 0)
                {
                    foreach (var i in transactions)
                    {
                        _total += i.Amount;
                    }
                }
                return Ok(new
                {
                    transactions,
                    total = _total
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

        [HttpPost("payos/service-package")]
        public async Task<IActionResult> CreatePaymentServicePackagePayOs(CreateUserPackage model)
        {
            try
            {
                var userPackage = await _userPackageService.CreateUserPackage(model);
                
                var url = await _payOsService.CreatePaymentLink(userPackage);
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

        [HttpPost("payos/confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(string url)
        {
            try
            {
                var result = await _payOsService.ConfirmWebhook(url);
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

        /// <summary>
        /// Callback endpoint for PayOs webhook notifications.
        /// Processes payment and updates the transaction status based on the webhook data received from PayOs.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("payos/webhook")]
        public async Task<IActionResult> PayOsWebhook([FromBody] WebhookType model)
        {
            try
            {
                
                var createTransactionDTO = await _payOsService.HandlePaymentWebhook(model);
                if (createTransactionDTO is null) return Ok("Check confirm");

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
    }
}
