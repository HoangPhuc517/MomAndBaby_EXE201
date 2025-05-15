using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Base;
using MomAndBaby.Services.DTO.DealModel;
using MomAndBaby.Services.DTO.ServicePackageModel;

namespace MomAndBaby.Services.Interface
{
    public interface IDealService
    {
        Task<PackageViewModel> CreateDeal(CreateDealModel dealModel);
        Task<PackageViewModel> UpdateDeal(string id, UpdateDealModel dealModel);
        Task UpdateStatusDeal(string id, BaseEnum statusEnum);
    }
}
