using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.DealModel;
using MomAndBaby.Services.DTO.ServicePackageModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class DealService : IDealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DealService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PackageViewModel> CreateDeal(CreateDealModel dealModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var deal = _mapper.Map<Deal>(dealModel);
                var check = await _unitOfWork.GenericRepository<Deal>()
                                             .GetFirstOrDefaultAsync(x => x.Name == dealModel.Name);
                if (check != null) throw new BaseException(StatusCodes.Status400BadRequest, "Deal name already exists!!!!");

                await _unitOfWork.GenericRepository<Deal>().InsertAsync(deal);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var package = await _unitOfWork.GenericRepository<ServicePackage>()
                                                  .GetFirstOrDefaultAsync(x => x.Id == deal.ServicePackageId, "Deals");
                var packageViewModel = _mapper.Map<PackageViewModel>(package);

                packageViewModel.Deals = packageViewModel.Deals
                                                         .Where(x => x.Status == BaseEnum.Active.ToString())
                                                         .ToList();

                return packageViewModel;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PackageViewModel> UpdateDeal(string id, UpdateDealModel dealModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var deal = await _unitOfWork.GenericRepository<Deal>()
                                             .GetFirstOrDefaultAsync(x => x.Id.ToString() == id);
                if (deal is null) throw new BaseException(StatusCodes.Status404NotFound, "Deal not found!!!");

                _mapper.Map(dealModel, deal);

                deal.UpdatedTime = DateTimeOffset.UtcNow;
                _unitOfWork.GenericRepository<Deal>().Update(deal);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var package = await _unitOfWork.GenericRepository<ServicePackage>()
                                               .GetFirstOrDefaultAsync(x => x.Id == deal.ServicePackageId, "Deals");

                var packageViewModel = _mapper.Map<PackageViewModel>(package);

                packageViewModel.Deals = packageViewModel.Deals
                                                         .Where(x => x.Status == BaseEnum.Active.ToString())
                                                         .ToList();

                return packageViewModel;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateStatusDeal(string id, BaseEnum statusEnum)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var deal = await _unitOfWork.GenericRepository<Deal>()
                                             .GetFirstOrDefaultAsync(x => x.Id.ToString() == id);
                if (deal is null) throw new BaseException(StatusCodes.Status404NotFound, "Deal not found!!!");

                deal.Status = statusEnum.ToString();

                _unitOfWork.GenericRepository<Deal>().Update(deal);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<DealViewModel>> GetDealAll()
        {
            try
            {
                var deals = _unitOfWork.GenericRepository<Deal>()
                                  .GetAll()
                                  .OrderByDescending(_ => _.CreatedTime);
                if (deals is null) throw new BaseException(StatusCodes.Status404NotFound, "Deal not found!!!");
                var dealViewModels = _mapper.Map<List<DealViewModel>>(deals);
                return dealViewModels;
            }
            catch
            {
                throw;
            }
        }
    }
}