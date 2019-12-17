using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> Login(LoginViewModel lvm);
    }
}
