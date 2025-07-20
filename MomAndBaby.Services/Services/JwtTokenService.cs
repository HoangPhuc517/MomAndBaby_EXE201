using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.UserModel;
using MomAndBaby.Services.Interface;
using StackExchange.Redis;

namespace MomAndBaby.Services.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public JwtTokenService(IConfiguration configuration, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]) ?? throw new Exception("JWT_KEY is not set");

            var fullName = user.FullName;

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("FullName", fullName),
        new Claim("Avatar", user.Avatar ?? ""),
    };

            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
                foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JWT:ValidIssuer"] ?? throw new Exception("JWT_ISSUER is not set"),
                Audience = _configuration["JWT:ValidAudience"] ?? throw new Exception("JWT_AUDIENCE is not set"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token));
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            string? refreshToken = Guid.NewGuid().ToString();

            string? initToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");
            if (initToken != null)
            {

                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");

            }

            await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
            return refreshToken;
        }

        public async Task<AuthenResponse> RefreshToken(string refreshToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);
                if (user == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Account not found!!!");
                }
                if (user.Status != BaseEnum.Active.ToString())
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "Access denied. Insufficient permissions!!!");
                }
                if (user.DateExpireRefreshToken < DateTimeOffset.UtcNow)
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "Refresh token expired!!!");
                }
                var newRefreshToken = await GenerateRefreshToken(user);
                var newAccessToken = await GenerateJwtToken(user);
                user.RefreshToken = newRefreshToken;
                user.DateExpireRefreshToken = DateTimeOffset.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return new AuthenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
