using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Services.DTO.UserModel;

namespace MomAndBaby.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<AuthenResponse> LoginAsync(LoginRequest model);
        Task<string> RegisterCustomerAsync(RegisterCustomerDTO model);
        Task<string> RegisterExpertAsync(RegisterExpertDTO model);
        Task ConfirmEmail(string email);
    }
}
