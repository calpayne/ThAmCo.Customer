using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Orders;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Reviews;

namespace ThAmCo.Customer.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IProductsService _products;
        private readonly IOrdersService _orders;
        private readonly IReviewsService _reviews;

        public ReviewsController(IProductsService products, IOrdersService orders, IReviewsService reviews)
        {
            _products = products;
            _orders = orders;
            _reviews = reviews;
        }

        private async Task<bool> checkCanReview(int productId)
        {
            string suid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            if (suid == null)
            {
                return false;
            }

            bool hasOredered = await _orders.CustomerHasOrderedAsync(productId, suid);

            if (!hasOredered)
            {
                return false;
            }

            return true;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            ProductDto product = await _products.GetByIDAsync(id);

            if (product == null)
            {
                return BadRequest();
            }

            if (User == null)
            {
                return BadRequest();
            }

            bool canReview = await checkCanReview(id);

            if (!canReview)
            {
                return BadRequest();
            }

            return View(new ReviewDto 
            { 
                ProductId = id
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title","Description","Rating","ProductId")] ReviewDto review)
        {
            ProductDto product = await _products.GetByIDAsync(review.ProductId);

            if (product == null)
            {
                return BadRequest();
            }

            if (User == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool canReview = await checkCanReview(review.ProductId);

                if (!canReview)
                {
                    TempData["error"] = "You cannot review this product.";
                    return RedirectToAction("Details", "Products", new { @id = review.ProductId });
                }

                var created = await _reviews.CreateReview(review);

                if (created)
                {
                    return RedirectToAction("Index", "Orders");
                }
            }

            return View(review);
        }
    }
}