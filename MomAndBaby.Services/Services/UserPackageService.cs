using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.UserPackageModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class UserPackageService : IUserPackageService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserPackageService(IMapper mapper, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserPackageViewModel> CreateUserPackage(CreateUserPackage createUserPackage)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var package = await _unitOfWork.GenericRepository<ServicePackage>()
                                               .GetFirstOrDefaultAsync(_ => _.Id.ToString() == createUserPackage.PackageId, "Deals");

                if (package is null || package.Price == 0 )
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Package not found Or Package is free!!!");
                }


                var userId = _currentUserService.GetUserId();

                var preUserPackage = await _unitOfWork.GenericRepository<UserPackage>()
                                                      .GetFirstOrDefaultAsync(_ => _.Status == BaseEnum.Active.ToString()
                                                                                       && _.UserId.ToString() == userId);
                if (preUserPackage != null)
                {
                    preUserPackage.Status = BaseEnum.Deleted.ToString();
                }
                var deal = new Deal();
                

                if (createUserPackage.DealId != null)
                {
                    deal = package.Deals.FirstOrDefault(_ => _.Id.ToString() == createUserPackage.DealId);

                    if (deal is null || deal.Status != BaseEnum.Active.ToString())
                    {
                        throw new BaseException(StatusCodes.Status404NotFound, "Deal not found!!!");
                    }
                    if (createUserPackage.MonthNumber % deal.OfferConditions != 0 || createUserPackage.MonthNumber == 0)
                    {
                        throw new BaseException(StatusCodes.Status422UnprocessableEntity,
                            $"This deal only applies to durations that are multiples of {deal.OfferConditions} months");
                    }
                }
                long orderCode;
                do
                {
                    orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                } while (_unitOfWork.GenericRepository<UserPackage>().GetAll().Any(_ => _.OrderCode == orderCode));

                var userPackage = new UserPackage()
                {
                    Status = BaseEnum.Pending.ToString(),
                    ExpiryDate = DateTimeOffset.UtcNow
                                               .AddMonths(createUserPackage.MonthNumber),
                    ValidMonths = createUserPackage.MonthNumber,
                    UsageCount = package.MonthlyUsageLimit,
                    OrderCode = orderCode,
                    UserId = Guid.Parse(userId),
                    ServicePackageId = package.Id,
                };


                await _unitOfWork.GenericRepository<UserPackage>().InsertAsync(userPackage);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var userPackageDB = await _unitOfWork.GenericRepository<UserPackage>()
                                               .GetFirstOrDefaultAsync(_ => _.Id == userPackage.Id, "ServicePackage");

                var result = _mapper.Map<UserPackageViewModel>(userPackageDB);

                result.Amount = result.ValidMonths * package.Price 
                              - (result.ValidMonths * package.Price * (decimal)deal.DiscountRate);

                return result;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserPackageViewModel> GetUserPackageById(string id)
        {
            try
            {
                var userPackage = await _unitOfWork.GenericRepository<UserPackage>()
                                             .GetFirstOrDefaultAsync(_ => _.Id.ToString() == id, "ServicePackage");

                var deal = await _unitOfWork.GenericRepository<Deal>()
                                            .GetFirstOrDefaultAsync(_ => _.Id.ToString() == userPackage.ServicePackageId.ToString() 
                                                                        && _.Status == BaseEnum.Active.ToString());

                if (userPackage is null) throw new BaseException(StatusCodes.Status404NotFound, "User package not found!!!");

                var userPackageViewModel = _mapper.Map<UserPackageViewModel>(userPackage);

                userPackageViewModel.Amount = userPackageViewModel.ValidMonths * userPackageViewModel.ServicePackage.Price
                              - (userPackageViewModel.ValidMonths * userPackageViewModel.ServicePackage.Price * (decimal)deal.DiscountRate);

                return userPackageViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserPackageViewModel> GetUserPackageByUserCurrent()
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                var package = await _unitOfWork.GenericRepository<UserPackage>()
                                             .GetFirstOrDefaultAsync(_ => _.UserId.ToString() == userId
                                                                        && _.Status == BaseEnum.Active.ToString()
                                                                    , "ServicePackage");

                if (package is null) throw new BaseException(StatusCodes.Status404NotFound, "User package not found!!!");

                var deal = await _unitOfWork.GenericRepository<Deal>()
                                            .GetFirstOrDefaultAsync(_ => _.Id.ToString() == package.ServicePackageId.ToString() && _.Status == BaseEnum.Active.ToString());

                var result = _mapper.Map<UserPackageViewModel>(package);

                result.Amount = result.ValidMonths * package.ServicePackage.Price
                              - (result.ValidMonths * package.ServicePackage.Price * (decimal)deal.DiscountRate);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UserPackageTotalCount()
        {
            try
            {
                return await _unitOfWork.GenericRepository<UserPackage>()
                                             .GetAll()
                                             .CountAsync(_ => _.Status != BaseEnum.Pending.ToString());
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<CalendarExpertViewModel>> GetCalendarExpertByExpertId(Guid expertId, int month, int year)
        {
            try
            {
                var timeStart = new DateTime(year, month, 1).Date;
                var timeEnd = timeStart.AddMonths(1).Date;
                var appointments = await _unitOfWork.GenericRepository<Appointment>()
                                                    .GetAllAsync(_ => _.ExpertId == expertId
                                                                      && _.Status == AppointmentStatusEnum.Approved.ToString()
                                                                      && _.AppointmentDate.Date >= timeStart
                                                                      && _.AppointmentDate.Date < timeEnd,
                                                                 "TimeSlot");
                if (!appointments.Any()) throw new BaseException(StatusCodes.Status404NotFound, "No appointments found for this expert in the specified month and year.");
                return _mapper.Map<List<CalendarExpertViewModel>>(appointments);
            }
            catch
            {
                throw;
            }
        }
    }
}
