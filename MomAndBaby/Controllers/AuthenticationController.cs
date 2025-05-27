using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Services.DTO.UserModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.API.Controllers
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtTokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        public AuthenticationController(IAuthenticationService authenticationService, IJwtTokenService jwtTokenService, ICurrentUserService currentUserService)
        {
            _authenticationService = authenticationService;
            _tokenService = jwtTokenService;
            _currentUserService = currentUserService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authenticationService.LoginAsync(request);
                if (result == null)
                {
                    throw new Exception("Login fail!!!");
                }
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDTO request)
        {
            try
            {
                var result = await _authenticationService.RegisterCustomerAsync(request);
                if (result == null)
                {
                    throw new Exception("Register fail!!!");
                }
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register/expert")]
        public async Task<IActionResult> RegisterExpert([FromBody] RegisterExpertDTO request)
        {
            try
            {
                var result = await _authenticationService.RegisterExpertAsync(request);
                if (result == null)
                {
                    throw new Exception("Register fail!!!");
                }
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDTO request)
        {
            try
            {
                var result = await _authenticationService.RegisterAdminAsync(request);
                if (result == null)
                {
                    throw new Exception("Register fail!!!");
                }
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                var result = await _tokenService.RefreshToken(refreshToken);
                if (result == null)
                {
                    throw new Exception("Refresh token fail!!!");
                }
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            try
            {
                await _authenticationService.ConfirmEmail(email);

                return Redirect("https://www.facebook.com/duong.hoai.ngan.2024");
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _authenticationService.LogoutAsync();
                return Ok("Logout successfully");
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var result = await _authenticationService.ForgotPasswod(email);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
        {
            try
            {
                await _authenticationService.ResetPassword(email, token, newPassword);
                return Ok("Reset password successfully");
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
