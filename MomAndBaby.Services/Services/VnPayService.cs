using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.DTO.VnPayModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public VnPayService(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public string CreateVNPayUrl(HttpContext context, VnPayRequestModel model)
        {
            try
            {
                var vnpay = new VnPayLibrary();
                var tick = DateTime.Now.Ticks.ToString();


                vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
                vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
                vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
                vnpay.AddRequestData("vnp_Amount", ((long)model.Amount * 100).ToString()); // Convert to integer amount (VNĐ)
                vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
                vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
                vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
                vnpay.AddRequestData("vnp_OrderInfo", $"{model.UserId} USERPACKAGE: {model.Id}");

                vnpay.AddRequestData("vnp_OrderType", "other"); // default value: other
                var callback = _config["VnPay:PaymentBackReturnUrl"];
                vnpay.AddRequestData("vnp_ReturnUrl", callback);
                vnpay.AddRequestData("vnp_TxnRef", tick); // Transaction reference ID, must be unique per day

                // Create the payment URL
                var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

                return paymentUrl;
            }
            catch
            {
                throw;
            }
        }

        public CreateTransactionDTO ProcessVNPayResponse(IQueryCollection collections)
        {
            try
            {
                var vnpay = new VnPayLibrary();
                foreach (var (key, value) in collections)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value.ToString());
                    }
                }
                var inforMessage = Utils.ExtractUserInfo(vnpay.GetResponseData("vnp_OrderInfo"));

                var userId = inforMessage.userId;
                var vnp_PayDate = vnpay.GetResponseData("vnp_PayDate");
                var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
                var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                var vnp_BankTranNo = vnpay.GetResponseData("vnp_BankTranNo");
                var vnp_Amount = vnpay.GetResponseData("vnp_Amount");
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
                
                if (!checkSignature)
                {
                    throw new BaseException(StatusCodes.Status402PaymentRequired, "CheckSignature was failed!!!");
                }
                return new CreateTransactionDTO
                {
                    UserPackageId = inforMessage.userPackage,
                    Amount = decimal.Parse(vnp_Amount) / 100,
                    Type = TransactionTypeEnum.VNPay.ToString(),
                    Message = vnp_OrderInfo,
                    TransferAccountName = "VNPAY",
                    TransferAccountNumber = vnp_BankTranNo,
                    CreatedTime = new DateTimeOffset(DateTime.ParseExact(vnp_PayDate, "yyyyMMddHHmmss", null),TimeSpan.FromHours(+7)),
                    UserId = Guid.Parse(userId),
                    Status = vnp_ResponseCode == "00" ? BaseEnum.Success.ToString() : BaseEnum.Failed.ToString(),
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
