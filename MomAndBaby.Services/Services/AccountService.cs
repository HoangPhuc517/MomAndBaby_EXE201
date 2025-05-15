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
using MomAndBaby.Services.DTO.UserModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UploadFile _hepperUploadImage;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UploadFile hepperUploadImage, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _hepperUploadImage = hepperUploadImage;
            _mapper = mapper;
        }
        public async Task<string> UpdateAvatar(User user, IFormFile file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                using var stream = file.OpenReadStream();
                var url = await _hepperUploadImage.UploadImageAsync(stream, file.FileName);

                user.Avatar = url;
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return user.Avatar;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            try
            {
                var user = await _unitOfWork.GenericRepository<User>()
                                            .GetFirstOrDefaultAsync(_ => _.Id.ToString() == userId,
                                                                    "Expert");
                if (user is null) throw new BaseException(StatusCodes.Status404NotFound, "User not found");
                var userViewModel = _mapper.Map<UserViewModel>(user);
                return userViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserViewModel> GetUserLoginCurrent()
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                var user = await _unitOfWork.GenericRepository<User>()
                                            .GetFirstOrDefaultAsync(_ => _.Id.ToString() == userId,
                                                                    "Expert");
                if (user is null) throw new BaseException(StatusCodes.Status404NotFound, "User not found");
                var userViewModel = _mapper.Map<UserViewModel>(user);
                return userViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<UserViewModel>> GetPaginationAsync(
            int pageIndex,
            int pageSize,
            string? fullName,
            string? orderBy,
            bool isDescending,
            string? includeProperties = "Expert",
            BaseEnum Status = BaseEnum.Active
            )
        {
            try
            {
                var paginationUser = await _unitOfWork.GenericRepository<User>()
                    .GetPaginationAsync(
                        predicate: _ => (string.IsNullOrEmpty(fullName) || _.FullName.Contains(fullName))
                                && (_.Status == Status.ToString()),
                        pageIndex: pageIndex,
                        pageSize: pageSize,
                        isDescending: isDescending,
                        includeProperties: includeProperties,
                        orderBy: _ => _.FullName
                    );
                if (paginationUser is null) throw new BaseException(StatusCodes.Status404NotFound, "List empty!!!");
                var paginationUserViewModel = _mapper.Map<Pagination<UserViewModel>>(paginationUser);
                return paginationUserViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserViewModel> UpdateUser(string id, UserUpdateDTO userUpdateDTO)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userDb = await _unitOfWork.GenericRepository<User>()
                                              .GetFirstOrDefaultAsync(_ => _.Id.ToString() == id, "Expert");
                if (userDb is null) throw new BaseException(StatusCodes.Status404NotFound, "User not found");

                _mapper.Map(userUpdateDTO, userDb);
                userDb.UpdatedTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
                _unitOfWork.GenericRepository<User>().Update(userDb);

                await _unitOfWork.SaveChangeAsync();
                var userViewModel = _mapper.Map<UserViewModel>(userDb);
                await _unitOfWork.CommitTransactionAsync();

                return userViewModel;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
