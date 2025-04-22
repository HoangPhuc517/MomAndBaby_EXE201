using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.Interface
{
    public interface ICurrentUserService
    {
        string GetUserId();
        string getUserEmail();
        Task<User> GetCurrentAccountAsync();
    }
}
