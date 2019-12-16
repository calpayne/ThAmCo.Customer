using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Services.Profiles;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IProfilesService _profiles;

        public AccountsController(IProfilesService profiles)
        {
            _profiles = profiles;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Username,Password")] LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                //do stuff
                return RedirectToAction(nameof(Login));
            }

            return View(lvm);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> Update()
        {
            var profile = await _profiles.GetProfileAsync(1);
            return View(UpdateViewModel.Transform(profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Id,FirstName,Surname,Email,TelNo,DeliverAddress1,DeliverAddress2,DeliverAddress3,Postcode")] UpdateViewModel uvm)
        {
            if (ModelState.IsValid)
            {
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