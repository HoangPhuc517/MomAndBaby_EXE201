using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.DTO.UserPackageModel;
using Net.payOS.Types;

namespace MomAndBaby.Services.Interface
{
    public interface IPayOsService
    {
        Task<string> CreatePaymentLink(UserPackageViewModel model);
        Task<CreateTransactionDTO> HandlePaymentWebhook(WebhookType webhookData);
        Task<string> ConfirmWebhook(string url);
    }
}
