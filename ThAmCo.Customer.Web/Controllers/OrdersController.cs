using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Services.Orders;

namespace ThAmCo.Customer.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _orders;

        public OrdersController(IOrdersService orders)
        {
            _orders = orders;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string suid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            if (suid == null)
            {
                return BadRequest();
            }

            return View(await _orders.GetOrdersAsync(suid));
        }
    }
}