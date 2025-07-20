using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.DTO.UserPackageModel;
using MomAndBaby.Services.Interface;
using Net.payOS;
using Net.payOS.Types;

namespace MomAndBaby.Services.Services
{
    public class PayOsService : IPayOsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly PayOS _payOS;
        private readonly ITransactionService _transactionService;
        private readonly ILogger<PayOsService> _logger;

        public PayOsService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, PayOS payOS, ITransactionService transactionService, ILogger<PayOsService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _payOS = payOS;
            _transactionService = transactionService;
            _logger = logger;
        }

        public async Task<string> ConfirmWebhook(string url)
        {
            try
            {
                return await _payOS.confirmWebhook(url);

            }
            catch (Exception exception)
            {

                Console.WriteLine(exception);
                return exception.Message;
            }
        }

        public async Task<string> CreatePaymentLink(UserPackageViewModel model)
        {
            try
            {
                var description = $"Mom And Baby App";

                //Tạo thông tin sản phẩm để hiển thị ở web thanh toán
                ItemData item = new ItemData(model.ServicePackage.Name, model.ValidMonths, (int)model.Amount);

                List<ItemData> items = new List<ItemData> { item };

                var baseUrl = "https://landing-page-exe-201.vercel.app";

                PaymentData paymentData = new PaymentData(  model.OrderCode.Value,
                                                            item.price,
                                                            description,
                                                            items,
                                                            $"{baseUrl}/cancel",
                                                            $"{baseUrl}/success"
                                                         );
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return createPayment.checkoutUrl;
            }
            catch
            {
                throw;
            }
            }

        public async Task<CreateTransactionDTO> HandlePaymentWebhook(WebhookType webhookData)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(webhookData);

                var userPackage = await _unitOfWork.GenericRepository<UserPackage>()
                                                   .GetFirstOrDefaultAsync(_ => _.OrderCode == data.orderCode);
                if (userPackage is null)
                {
                    _logger.LogError("Confirm!!!---------------");
                    return null;
                }

                    CreateTransactionDTO transactionDTO = new CreateTransactionDTO
                {
                    UserPackageId = userPackage.Id.ToString(),
                    Amount = data.amount,
                    Type = data.counterAccountBankName ?? "PayOS",
                    Message = data.description,
                    TransferAccountName = data.counterAccountName ?? "PayOS",
                    TransferAccountNumber = data.counterAccountNumber ?? "PayOS",
                    CreatedTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)),
                    UserId = userPackage.UserId,
                    Status = webhookData.success ? BaseEnum.Success.ToString() : BaseEnum.Failed.ToString()

                };
                _logger.LogInformation("Payment webhook data verified successfully.");
                return transactionDTO;
            }
            catch
            {
                throw;
            }
        }
    }
}
