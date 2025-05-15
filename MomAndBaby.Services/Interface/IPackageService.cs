using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.ServicePackageModel;

namespace MomAndBaby.Services.Interface
{
    public interface IPackageService
    {
        Task<PackageViewModel> CreateServicePackage(PackageModel packageModel);
        Task UpdateStatusPackage(string id, BaseEnum statusEnum);
        Task<PackageViewModel> UpdatePackage(string id, UpdatePackageModel packageModel);
        Task<Pagination<PackageViewModel>> GetPackageByPagination(int pageIndex, int pageSize, string? searchNamePackage, PagingPackageEnum pagingPackageEnum, bool isDescending);
        Task<PackageViewModel> GetPackageById(string id);
    }
}
