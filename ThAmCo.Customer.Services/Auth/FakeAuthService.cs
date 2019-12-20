using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Auth
{
    public class FakeAuthService : IAuthService
    {
        public Task<AuthDto> Login(LoginViewModel lvm)
        {
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "f32d935b-f175-4450-a93e-e48711c4d481"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsIdentity = new ClaimsIdentity(fakeClaims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var tokensToStore = new AuthenticationToken[]
            {
                new AuthenticationToken { Name = "access_token", Value = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFmYzVmM2YzZDI3MDBlM2E2YjA3ZDk5MDBmMDAzYTZjIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzY4NzYxNzIsImV4cCI6MTU3Njg3OTc3MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTA5OSIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo1MDk5L3Jlc291cmNlcyIsInRoYW1jb19hY2NvdW50X2FwaSJdLCJjbGllbnRfaWQiOiJ0aGFtY29fY3VzdG9tZXJfd2ViIiwic3ViIjoiZjMyZDkzNWItZjE3NS00NDUwLWE5M2UtZTQ4NzExYzRkNDgxIiwiYXV0aF90aW1lIjoxNTc2ODc2MTcxLCJpZHAiOiJsb2NhbCIsInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiLCJyb2xlcyIsInRoYW1jb19hY2NvdW50X2FwaSJdLCJhbXIiOlsicHdkIl19.a1oysHJuN0IbxlUUCKbesQL5zkApIkprplaPzCLn0AhSbOkHKcDjTBz2Km6Mbn_oI6mvvUctQJkTfukFVcw4VvRN_iQLS2ZKr2QN1RxxS5epJkQiO_6I0WGsWJyPac-FzgAJ8ZgOHtMW9R-ZYvMERXQDeoB9gYiglS3u37ILh58sy_CCTlrCISA-mITbLgUzaN4fm-Ihwgnzc9AdVkDJ_4V9U2gLQlOPRT6-Q6owez2FqjaYwePHxHJEeB3XlLJsInTKbiCTNP2XZVD7GGikH3S5WNzm75pQ1HrCsHVx_M4AfTJUEcknlCLFdMvpwsFHV4_Zgk6bDZNJEp9IdwHCrQ" }
            };

            return Task.FromResult(new AuthDto
            {
                claimsPrincipal = claimsPrincipal,
                tokensToStore = tokensToStore
            });
        }
    }
}
