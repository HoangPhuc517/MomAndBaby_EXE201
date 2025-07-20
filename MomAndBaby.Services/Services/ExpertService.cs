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
using MomAndBaby.Repositories.Repositories;
using MomAndBaby.Services.DTO.ExpertModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;
using static StackExchange.Redis.Role;

namespace MomAndBaby.Services.Services
{
    public class ExpertService : IExpertService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public readonly IMapper _mapper;
        public ExpertService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ExpertProfileViewModel> GetByIdAsync(string expertIdOrUserId)
        {
            try
            {
                var expertDb = await _unitOfWork.GenericRepository<Expert>()
                    .GetFirstOrDefaultAsync(
                        predicate: x => x.UserId.ToString() == expertIdOrUserId || x.Id.ToString() == expertIdOrUserId,
                        includeProperties: "User"
                    );
                if (expertDb is null) throw new BaseException(StatusCodes.Status404NotFound, "Expert not found");
                var expertViewModel = _mapper.Map<ExpertProfileViewModel>(expertDb);
                return expertViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<ExpertProfileViewModel>> GetPaginationAsync(string? searchString, int pageIndex, int pageSize, bool isDescending, BaseEnum Status = BaseEnum.Active)
        {
            try
            {
                var expertDbList = await _unitOfWork.GenericRepository<Expert>()
                    .GetPaginationAsync(
                        predicate: x => x.Status == Status.ToString()
                            && (string.IsNullOrEmpty(searchString)
                                || x.User.FullName.Contains(searchString)
                                || x.Workplace.Contains(searchString)
                                || x.Specialty.Contains(searchString))
                            && (x.Status == Status.ToString()),
                        includeProperties: "User",
                        pageIndex: pageIndex,
                        pageSize: pageSize,
                        orderBy: x => x.Stars,
                        isDescending: isDescending
                    );

                if (expertDbList is null) throw new BaseException(StatusCodes.Status404NotFound, "Expert not found");

                var expertViewModelList = _mapper.Map<Pagination<ExpertProfileViewModel>>(expertDbList);
                return expertViewModelList;

            }
            catch
            {
                throw;
            }
        }

        public async Task<ExpertProfileViewModel> UpdateExpert(string id, UpdateExpertModel model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var expertDb = await _unitOfWork.GenericRepository<Expert>()
                    .GetFirstOrDefaultAsync(
                        predicate: x => x.Id.ToString() == id
                    );
                if (expertDb is null) throw new BaseException(StatusCodes.Status404NotFound, "Expert not exist!!!");
                _mapper.Map(model, expertDb);
                expertDb.UpdatedTime = DateTimeOffset.UtcNow;
                _unitOfWork.GenericRepository<Expert>().Update(expertDb);
                await _unitOfWork.SaveChangeAsync();
                var expertViewModel = _mapper.Map<ExpertProfileViewModel>(expertDb);
                await _unitOfWork.CommitTransactionAsync();
                return expertViewModel;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ExpertProfileViewModel> UpdateStatusExpert(string id, BaseEnum statusRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var expertDb = await _unitOfWork.GenericRepository<Expert>()
                    .GetFirstOrDefaultAsync(
                        predicate: x => x.Id.ToString() == id
                    );
                if (expertDb is null) throw new BaseException(StatusCodes.Status404NotFound, "Expert not exist!!!");
                expertDb.Status = statusRequest.ToString();
                expertDb.UpdatedTime = DateTimeOffset.UtcNow;
                _unitOfWork.GenericRepository<Expert>().Update(expertDb);
                await _unitOfWork.SaveChangeAsync();
                var expertViewModel = _mapper.Map<ExpertProfileViewModel>(expertDb);
                await _unitOfWork.CommitTransactionAsync();
                return expertViewModel;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
