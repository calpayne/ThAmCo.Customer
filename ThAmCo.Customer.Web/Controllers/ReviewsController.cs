using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Reviews;

namespace ThAmCo.Customer.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IProductsService _products;
        private readonly IReviewsService _reviews;

        public ReviewsController(IProductsService products, IReviewsService reviews)
        {
            _products = products;
            _reviews = reviews;
        }

        public async Task<IActionResult> Create(int id)
        {
            ProductDto product = await _products.GetByIDAsync(id);

            if (product == null)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title","Description","Rating","ProductId")] ReviewDto review)
        {
            if (ModelState.IsValid)
            {
                //check that they ordered the product
                //send their review to reviews api
                return RedirectToAction(nameof(Create));
            }

            return View(review);
        }
    }
}