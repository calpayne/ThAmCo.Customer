using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class AuthDto
    {
        public AuthenticationToken[] tokensToStore { get; set; }

        public ClaimsPrincipal claimsPrincipal { get; set; }
    }
}
