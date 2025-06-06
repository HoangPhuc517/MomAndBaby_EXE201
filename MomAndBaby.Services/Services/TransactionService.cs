using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTransaction(CreateTransactionDTO model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (model.Status == BaseEnum.Success.ToString())
                {
                    var userPackage = await _unitOfWork.GenericRepository<UserPackage>()
                                   .GetFirstOrDefaultAsync(_ => _.Id.ToString() == model.UserPackageId);

                    if (userPackage is null) throw new BaseException(StatusCodes.Status404NotFound, "User package not found");


                    userPackage.UpdatedTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
                    userPackage.Status = BaseEnum.Active.ToString();
                    _unitOfWork.GenericRepository<UserPackage>().Update(userPackage);
                }

                var transaction = _mapper.Map<Transaction>(model);
                transaction.UpdatedTime = transaction.CreatedTime;
                await _unitOfWork.GenericRepository<Transaction>().InsertAsync(transaction);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<TransactionViewModel> GetTransactionById(string id)
        {
            try
            {
                var transaction = await _unitOfWork.GenericRepository<Transaction>()
                    .GetFirstOrDefaultAsync(_ => _.Id.ToString() == id);
                if (transaction is null) throw new BaseException(StatusCodes.Status404NotFound, "Transaction not found");
                var transactionViewModel = _mapper.Map<TransactionViewModel>(transaction);
                return transactionViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<TransactionViewModel>> GetTransactionByPaging(int pageIndex, int pageSize, string? stringSearch, DateTime? startDate, DateTime? endDate, bool isDescending, string? userId)
        {
            try
            {
                var transactionList = await _unitOfWork
                                        .GenericRepository<Transaction>()
                                        .GetPaginationAsync
                                        (
                                            predicate: _ => (string.IsNullOrEmpty(userId) || _.UserId.ToString() == userId)
                                                        && (string.IsNullOrEmpty(stringSearch) || _.Message.Contains(stringSearch) || _.Type.Contains(stringSearch) || _.TransferAccountNumber.Contains(stringSearch) || _.TransferAccountName.Contains(stringSearch))
                                                        && (!startDate.HasValue || _.CreatedTime >= startDate)
                                                        && (!endDate.HasValue || _.CreatedTime <= endDate),
                                            pageIndex: pageIndex,
                                            pageSize: pageSize,
                                            orderBy: _ => _.CreatedTime,
                                            isDescending: isDescending
                                        );
                if (transactionList is null) throw new BaseException(StatusCodes.Status404NotFound, "Transaction not found");
                var transactionViewModelList = _mapper.Map<Pagination<TransactionViewModel>>(transactionList);
                return transactionViewModelList;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TransactionViewModel>> GetTransactionByMonth(int month, int year, string? userId)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1);

                var transactions = await _unitOfWork.GenericRepository<Transaction>()
                    .GetAllAsync(filter: _ => _.CreatedTime >= startDate 
                                              && _.CreatedTime < endDate 
                                              && (string.IsNullOrEmpty(userId) || _.UserId.ToString() == userId),
                                 includeProperties: null);
                if (transactions is null) 
                    throw new BaseException(StatusCodes.Status404NotFound, "Transaction not found for the specified month and year");
                var transactionViewModels = _mapper.Map<List<TransactionViewModel>>(transactions);
                return transactionViewModels;
            }
            catch
            {
                throw;
            }
        }
    }
}
