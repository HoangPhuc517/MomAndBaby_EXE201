using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.TransactionModel;

namespace MomAndBaby.Services.Interface
{
    public interface ITransactionService
    {
        Task CreateTransaction(CreateTransactionDTO model);
        Task<Pagination<TransactionViewModel>> GetTransactionByPaging( int pageIndex, int pageSize, string? stringSearch, DateTime? startDate, DateTime? endDate, bool isDescending, string? userId);
        Task<TransactionViewModel> GetTransactionById(string id);
        Task<List<TransactionViewModel>> GetTransactionByMonth(int month, int year, string? userId);
    }
}
