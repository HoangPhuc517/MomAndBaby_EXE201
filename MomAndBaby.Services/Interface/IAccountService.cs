using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.UserModel;

namespace MomAndBaby.Services.Interface
{
    public interface IAccountService
    {
        Task<string> UpdateAvatar(User user, IFormFile file);
        Task<UserViewModel> GetUserById(string userId);
        Task<UserViewModel> GetUserLoginCurrent();
        Task<UserViewModel> UpdateUser(string id, UserUpdateDTO userUpdateDTO);
        Task<Pagination<UserViewModel>> GetPaginationAsync(
            int pageIndex,
            int pageSize,
            string? fullName,
            string? orderBy,
            bool isDescending,
            string? includeProperties = "Expert",
            BaseEnum Status = BaseEnum.Active
            );
    }
}
