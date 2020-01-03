using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using ThAmCo.Customer.Models;
using System.Security.Claims;
using System.Net;

namespace ThAmCo.Customer.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public AuthService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<AuthDto> Login(LoginViewModel lvm)
        {
            var disco = await _client.GetDiscoveryDocumentAsync(_config["AuthServer"]);
            var tokenResponse = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _config["ClientId"],
                ClientSecret = _config["ClientSecret"],

                UserName = lvm.Email,
                Password = lvm.Password
            });

            if (tokenResponse.IsError)
            {
                return null;
            }

            var userInfoResponse = await _client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            });

            if (userInfoResponse.IsError)
            {
                return null;
            }

            var claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var tokensToStore = new AuthenticationToken[]
            {
                new AuthenticationToken { Name = "access_token", Value = tokenResponse.AccessToken }
            };

            return new AuthDto { 
                claimsPrincipal = claimsPrincipal,
                tokensToStore = tokensToStore
            };
        }

        public async Task<bool> Register(RegisterViewModel rvm)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/accounts/", rvm);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsAsync<RegisterViewModel>();

                if (data == null || data.EmailAddress != rvm.EmailAddress)
                {
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }

            return true;
        }
    }
}
