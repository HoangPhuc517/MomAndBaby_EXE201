using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.ServicePackageModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PackageViewModel> CreateServicePackage(PackageModel packageModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var check = await _unitOfWork.GenericRepository<ServicePackage>()
                    .GetFirstOrDefaultAsync(x => x.Name == packageModel.Name);
                if (check != null)
                {
                    throw new BaseException(StatusCodes.Status400BadRequest, "Package name already exists!!!!");
                }
                var package = _mapper.Map<ServicePackage>(packageModel);

                await _unitOfWork.GenericRepository<ServicePackage>().InsertAsync(package);
                await _unitOfWork.SaveChangeAsync();

                var packageViewModel = _mapper.Map<PackageViewModel>(package);

                await _unitOfWork.CommitTransactionAsync();
                return packageViewModel;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PackageViewModel> UpdatePackage(string id, UpdatePackageModel packageModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var package = await _unitOfWork.GenericRepository<ServicePackage>()
                                               .GetFirstOrDefaultAsync(x => x.Id.ToString() == id);

                if (package is null) throw new BaseException(StatusCodes.Status404NotFound, "Package not found");

                
                _mapper.Map(packageModel, package);
                package.UpdatedTime = DateTimeOffset.UtcNow;

                _unitOfWork.GenericRepository<ServicePackage>().Update(package); 

                await _unitOfWork.SaveChangeAsync();

                var packageViewModel = _mapper.Map<PackageViewModel>(package);
                packageViewModel.Deals = packageViewModel.Deals
                                                         .Where(x => x.Status == BaseEnum.Active.ToString())
                                                         .ToList();
                await _unitOfWork.CommitTransactionAsync();
                return packageViewModel;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateStatusPackage(string id, BaseEnum statusEnum)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var package = await _unitOfWork.GenericRepository<ServicePackage>()
                                               .GetFirstOrDefaultAsync(x => x.Id.ToString() == id);

                if (package is null) throw new BaseException(StatusCodes.Status404NotFound, "Package not found");

                package.Status = statusEnum.ToString();
                package.UpdatedTime = DateTimeOffset.UtcNow;

                _unitOfWork.GenericRepository<ServicePackage>()
                           .Update(package);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Pagination<PackageViewModel>> GetPackageByPagination(int pageIndex, int pageSize, string? searchNamePackage, PagingPackageEnum pagingPackageEnum, bool isDescending)
        {
            try
            {
                var packageDbList = await _unitOfWork.GenericRepository<ServicePackage>()
                                                     .GetPaginationAsync(
                                                        predicate: x => (string.IsNullOrEmpty(searchNamePackage)
                                                                        || x.Name.Contains(searchNamePackage)
                                                        ),
                                                        pageIndex: pageIndex,
                                                        pageSize: pageSize,
                                                        includeProperties: "Deals"
                                                     );
                if (packageDbList is null) throw new BaseException(StatusCodes.Status404NotFound, "Packages empty!!!");

                var packageViewModelList = _mapper.Map<Pagination<PackageViewModel>>(packageDbList);

                if (pagingPackageEnum == PagingPackageEnum.Name)
                {
                    packageViewModelList.Items = isDescending ?
                                                    packageViewModelList.Items
                                                        .OrderByDescending(x => x.Name)
                                                        .ToList() :
                                                    packageViewModelList.Items
                                                        .OrderBy(x => x.Name)
                                                        .ToList();
                }
                else if (pagingPackageEnum == PagingPackageEnum.Price)
                {
                    packageViewModelList.Items = isDescending ?
                                                    packageViewModelList.Items
                                                        .OrderByDescending(x => x.Price)
                                                        .ToList() :
                                                    packageViewModelList.Items
                                                        .OrderBy(x => x.Price)
                                                        .ToList();
                }
                else if (pagingPackageEnum == PagingPackageEnum.CreateTime)
                {
                    packageViewModelList.Items = isDescending ?
                                                    packageViewModelList.Items
                                                        .OrderByDescending(x => x.CreatedTime)
                                                        .ToList() :
                                                    packageViewModelList.Items
                                                        .OrderBy(x => x.CreatedTime)
                                                        .ToList();
                }
                foreach( var item in packageViewModelList.Items)
                {
                   item.Deals = item.Deals
                                    .Where(x => x.Status == BaseEnum.Active.ToString())
                                    .ToList();
                }
                return packageViewModelList;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PackageViewModel> GetPackageById(string id)
        {
            try
            {
                var package = await _unitOfWork.GenericRepository<ServicePackage>()
                                               .GetFirstOrDefaultAsync(x => x.Id.ToString() == id, "Deals");
                if (package is null) throw new BaseException(StatusCodes.Status404NotFound, "Package not found!!!");
                var packageViewModel = _mapper.Map<PackageViewModel>(package);
                packageViewModel.Deals = packageViewModel.Deals
                                                         .Where(x => x.Status == BaseEnum.Active.ToString())
                                                         .ToList();
                return packageViewModel;
            }
            catch
            {
                throw;
            }
        }
    }
}
