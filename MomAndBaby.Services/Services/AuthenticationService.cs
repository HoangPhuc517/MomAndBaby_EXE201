using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.UserModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class AuthenticationService : Interface.IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;

        public AuthenticationService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, RoleManager<IdentityRole<Guid>> roleManager, IJwtTokenService jwtTokenService, IEmailService emailService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _currentUserService = currentUserService;
        }
        public async Task<string> RegisterCustomerAsync(RegisterCustomerDTO model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Email already exists");
                }
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Username already exists");
                }
                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Phone number already exists");
                }

                var userDb = _mapper.Map<User>(model);
                userDb.Status = BaseEnum.Active.ToString();
                userDb.CreatedTime = DateTimeOffset.UtcNow;
                userDb.UpdatedTime = userDb.CreatedTime;

                var result = await _userManager.CreateAsync(userDb, model.Password);

                if (result.Succeeded)
                {
                    bool roleExist = await _roleManager.RoleExistsAsync(UserRoleEnum.CUSTOMER.ToString());
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = UserRoleEnum.CUSTOMER.ToString() });
                    }
                    await _userManager.AddToRoleAsync(userDb, UserRoleEnum.CUSTOMER.ToString());
                    await _emailService.SendMailRegister(userDb.Email);

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BaseException(StatusCodes.Status400BadRequest, errors);
                }

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return userDb.Id.ToString();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<string> RegisterExpertAsync(RegisterExpertDTO model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Email already exists");
                }
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Username already exists");
                }
                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Phone number already exists");
                }

                var userDb = _mapper.Map<User>(model);
                userDb.Status = BaseEnum.Active.ToString();
                userDb.CreatedTime = DateTimeOffset.UtcNow;
                userDb.UpdatedTime = userDb.CreatedTime;
                userDb.Expert.Status = BaseEnum.Pending.ToString();

                var result = await _userManager.CreateAsync(userDb, model.Password);

                if (result.Succeeded)
                {
                    bool roleExist = await _roleManager.RoleExistsAsync(UserRoleEnum.EXPERT.ToString());
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = UserRoleEnum.EXPERT.ToString() });
                    }
                    await _userManager.AddToRoleAsync(userDb, UserRoleEnum.EXPERT.ToString());
                    await _emailService.SendMailRegister(userDb.Email);

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BaseException(StatusCodes.Status400BadRequest, errors);
                }

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return userDb.Id.ToString();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<string> RegisterAdminAsync(RegisterAdminDTO model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Email already exists");
                }
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Username already exists");
                }
                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Phone number already exists");
                }

                var userDb = _mapper.Map<User>(model);
                userDb.Status = BaseEnum.Active.ToString();
                userDb.CreatedTime = DateTimeOffset.UtcNow;
                userDb.UpdatedTime = userDb.CreatedTime;

                var result = await _userManager.CreateAsync(userDb, model.Password);

                if (result.Succeeded)
                {
                    bool roleExist = await _roleManager.RoleExistsAsync(UserRoleEnum.ADMIN.ToString());
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = UserRoleEnum.ADMIN.ToString() });
                    }
                    await _userManager.AddToRoleAsync(userDb, UserRoleEnum.ADMIN.ToString());

                    await _emailService.SendMailRegister(userDb.Email);

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BaseException(StatusCodes.Status400BadRequest, errors);
                }

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return userDb.Id.ToString();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<AuthenResponse> LoginAsync(LoginRequest model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Account not found!!!");
                }

                if (user.LockoutEnd >= DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)))
                {
                    var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                    var lockoutTime = lockoutEnd?.UtcDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                    throw new BaseException(StatusCodes.Status403Forbidden, $"Your account is temporarily locked. Please try again after: {lockoutTime}");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
                if (result.IsLockedOut)
                {
                    var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                    var lockoutTime = lockoutEnd?.UtcDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                    throw new BaseException(StatusCodes.Status403Forbidden, $"Your account is temporarily locked. Please try again after: {lockoutTime}");
                }
                if (!result.Succeeded)
                {
                    throw new BaseException(StatusCodes.Status401Unauthorized, "Invalid password!!!");
                }

                if (user.Status != BaseEnum.Active.ToString())
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "Access denied. Insufficient permissions!!!");
                }

                if (user.EmailConfirmed is false)
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "Email not confirmed!!!");
                }

                string token = await _jwtTokenService.GenerateJwtToken(user);
                string refreshToken = await _jwtTokenService.GenerateRefreshToken(user);
                user.RefreshToken = refreshToken;
                user.DateExpireRefreshToken = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)).AddDays(7);

                _unitOfWork.GenericRepository<User>().Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new AuthenResponse
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task ConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new BaseException(StatusCodes.Status404NotFound, "Account not found!!!");
            }
            if (user.EmailConfirmed)
            {
                throw new BaseException(StatusCodes.Status400BadRequest, "Email already confirmed!!!");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new BaseException(StatusCodes.Status400BadRequest, "Confirm email failed!!!");
            }
        }

        public async Task LogoutAsync()
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _currentUserService.GetCurrentAccountAsync();
                if (user is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Account not found!!!");
                }
                user.RefreshToken = null;
                user.DateExpireRefreshToken = null;
                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<string> ForgotPasswod(string email)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Account not found!!!");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var plainTextBytes = Encoding.UTF8.GetBytes(token);
                var encodedToken = Base64UrlTextEncoder.Encode(plainTextBytes);
                await _emailService.SendMailForgotPassword(email, encodedToken);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return encodedToken;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        
        public async Task ResetPassword(string email, string token, string newPassword)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Account not found!!!");
                }

                var base64EncodedBytes = Convert.FromBase64String(token);
                var tokenEnCode = Encoding.UTF8.GetString(base64EncodedBytes);

                var result = await _userManager.ResetPasswordAsync(user, tokenEnCode, newPassword);
                if (!result.Succeeded)
                {
                    throw new BaseException(StatusCodes.Status400BadRequest, "Reset password failed!!!");
                }
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
