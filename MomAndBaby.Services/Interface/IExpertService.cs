using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.ExpertModel;
using MomAndBaby.Services.DTO.UserModel;

namespace MomAndBaby.Services.Interface
{
    public interface IExpertService
    {
        Task<Pagination<ExpertProfileViewModel>> GetPaginationAsync(
            string? searchString,
            int pageIndex,
            int pageSize,
            bool isDescending,
            BaseEnum Status = BaseEnum.Active
        );
        Task<ExpertProfileViewModel> GetByIdAsync(string expertIdOrUserId);
        Task<ExpertProfileViewModel> UpdateExpert(string id, UpdateExpertModel model);
        Task<ExpertProfileViewModel> UpdateStatusExpert(string id, BaseEnum statusRequest);
    }
}
