using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> Login(LoginViewModel lvm);
        Task<bool> Register(RegisterViewModel rvm);
        Task<string> GetProductsToken();
        Task<string> GetProfilesToken();
        Task<string> GetOrdersToken();
        Task<string> GetReviewsToken();
    }
}
