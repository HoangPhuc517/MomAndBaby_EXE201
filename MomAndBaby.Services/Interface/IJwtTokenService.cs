using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Services.DTO.UserModel;

namespace MomAndBaby.Services.Interface
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtToken(User user);
        Task<string> GenerateRefreshToken(User user);
        Task<AuthenResponse> RefreshToken(string refreshToken);
    }
}
