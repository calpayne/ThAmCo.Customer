using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            return View(await _orders.GetOrdersAsync(1));
        }
    }
}