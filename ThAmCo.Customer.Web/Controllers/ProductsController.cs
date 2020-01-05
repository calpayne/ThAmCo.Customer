using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Brands;
using ThAmCo.Customer.Services.Categories;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Profiles;
using ThAmCo.Customer.Services.Reviews;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _products;
        private readonly IBrandsService _brands;
        private readonly ICategoriesService _categories;
        private readonly IReviewsService _reviews;
        private readonly IProfilesService _profiles;

        public ProductsController(IProductsService products, IBrandsService brands, ICategoriesService categories, IReviewsService reviews, IProfilesService profiles)
        {
            _products = products;
            _brands = brands;
            _categories = categories;
            _reviews = reviews;
            _profiles = profiles;
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: Products
        public async Task<ActionResult> Index(int[] brands, int[] categories, string term, double? minPrice, double? maxPrice)
        {
            var products = await _products.GetAllAsync(brands, categories, term, minPrice, maxPrice);

            if (products == null)
            {
                products = Array.Empty<ProductDto>();
            }

            return View(new ProductsIndexViewModel
            {
                Brands = await _brands.GetAllAsync(),
                Categories = await _categories.GetAllAsync(),
                Products = products,
                IsLoggedIn = (User == null) ? false : User.Identity.IsAuthenticated
            });
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var product = await _products.GetByIDAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductsDetailsViewModel
            {
                Product = product,
                Reviews = await _reviews.GetAllAsync(product.Id),
                IsLoggedIn = (User == null) ? false : User.Identity.IsAuthenticated
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Purchase([Bind("Id")] PurchaseViewModel purchase)
        {
            var product = await _products.GetByIDAsync(purchase.Id);

            if (product == null)
            {
                return BadRequest();
            }

            if (User == null)
            {
                return BadRequest();
            }

            string usid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            bool canPurchase = await _profiles.CanPurchase(usid);
            if (!canPurchase)
            {
                TempData["error"] = "You cannot purchase products. Please make sure that your account information is complete.";
                return RedirectToAction("Details", new { @id = purchase.Id });
            }

            var success = await _products.PurchaseAsync(new OrderDto
            {
                Product = product,
                Customer = new CustomerDto
                {
                    Id = usid,
                    Email = User.Claims.FirstOrDefault(c => c.Type == "preferred_username").Value,
                    Name = User.Claims.FirstOrDefault(c => c.Type == "name").Value
                }
            });

            if(success)
            {
                return RedirectToAction("Index", "Orders");
            }

            TempData["error"] = "Your purchase failed. Please try again.";
            return RedirectToAction("Details", new { @id = purchase.Id });
        }
    }
}