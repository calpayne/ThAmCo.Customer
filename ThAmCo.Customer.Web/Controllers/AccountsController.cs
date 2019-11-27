using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Web.Controllers
{
    public class AccountsController : Controller
    {
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

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([Bind("FirstName,Surname,Email,TelNo,DeliverAddress1,DeliverAddress2,DeliverAddress3,Postcode")] UpdateViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                //do stuff
                return RedirectToAction(nameof(Update));
            }

            return View(uvm);
        }
    }
}