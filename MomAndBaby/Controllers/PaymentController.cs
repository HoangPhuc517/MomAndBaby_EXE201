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
                return Redirect("https://www.facebook.com/duong.hoai.ngan.2024");
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
