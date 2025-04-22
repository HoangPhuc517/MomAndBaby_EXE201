using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CurrentUserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetCurrentAccountAsync()
        {
            try
            {
                string userId = GetUserId();
                var account = await _unitOfWork.GenericRepository<User>().GetByIdAsync(Guid.Parse(userId));
                return account;
            }
            catch
            {
                throw;
            }

        }

        public string getUserEmail()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            }
            catch
            {
                throw;
            }
        }

        public string GetUserId()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new();
            }
            catch
            {
                throw new BaseException(StatusCodes.Status401Unauthorized, "Login Before USE!!!!");
            }
        }
    }
}
