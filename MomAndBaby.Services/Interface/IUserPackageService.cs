using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Services.DTO.UserPackageModel;

namespace MomAndBaby.Services.Interface
{
    public interface IUserPackageService
    {
        Task<UserPackageViewModel> CreateUserPackage(CreateUserPackage createUserPackage);
        Task<UserPackageViewModel> GetUserPackageByUserCurrent();
        Task<UserPackageViewModel> GetUserPackageById(string id);
    }
}
