using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.VnPayModel
{
    public class VNPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string Id { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }


    }
    public class TransactionResponseModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Transaction_Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
