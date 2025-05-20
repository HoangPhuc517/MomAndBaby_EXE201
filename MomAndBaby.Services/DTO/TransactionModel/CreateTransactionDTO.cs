using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.TransactionModel
{
    public class CreateTransactionDTO
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string? Message { get; set; }
        public string TransferAccountName { get; set; }
        public string TransferAccountNumber { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public string UserPackageId { get; set; }
    }
}
