using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Auth;
using ThAmCo.Customer.Services.Profiles;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IProfilesService _profiles;
        private readonly IAuthService _auth;

        public AccountController(IProfilesService profiles, IAuthService auth)
        {
            _profiles = profiles;
            _auth = auth;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect("/");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel lvm, [FromQuery] string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                AuthDto auth = await _auth.Login(lvm);

                if (auth == null)
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
                    return View(lvm);
                }

                var authProperties = new AuthenticationProperties();
                authProperties.StoreTokens(auth.tokensToStore);

                await HttpContext.SignInAsync("Cookies", auth.claimsPrincipal, authProperties);

                return LocalRedirect(returnUrl ?? "/");
            }

            return View(lvm);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout([FromQuery] string returnUrl = null)
        {
            await HttpContext.SignOutAsync("Cookies");

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect("/");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Username,EmailAddress,Password")] RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                //do stuff
                return RedirectToAction(nameof(Register));
            }

            return View(rvm);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update()
        {
            if (User == null)
            {
                return BadRequest();
            }

            string suid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            var profile = await _profiles.GetProfileAsync(suid);
            return View(UpdateViewModel.Transform(profile));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Id,FirstName,Surname,Email,TelNo,DeliverAddress1,DeliverAddress2,DeliverAddress3,Postcode")] UpdateViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                if (User == null)
                {
                    return BadRequest();
                }

                string suid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
                if (suid != uvm.Id)
                {
                    return BadRequest();
                }

                bool updated = await _profiles.UpdateProfileAsync(UpdateViewModel.ToProfileDto(uvm));

                if (!updated)
                {
                    return View(uvm);
                }

                return RedirectToAction(nameof(Update));
            }

            return View(uvm);
        }
    }
}