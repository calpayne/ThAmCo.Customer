using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Brands;
using ThAmCo.Customer.Services.Categories;
using ThAmCo.Customer.Services.Products;
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

        public ProductsController(IProductsService products, IBrandsService brands, ICategoriesService categories, IReviewsService reviews)
        {
            _products = products;
            _brands = brands;
            _categories = categories;
            _reviews = reviews;
        }

        // GET: Products
        public async Task<ActionResult> Index(int[] brands, int[] categories, string term, double? minPrice, double? maxPrice)
        {
            var products = await _products.GetAllAsync(brands, categories, term, minPrice, maxPrice);

            if (products == null)
            {
                products = Array.Empty<ProductDto>();
            }

            var user = HttpContext.User;
            var claims = user.Claims.ToArray();

            if (user.Identity.IsAuthenticated)
            {
                string suid = claims[0].Value;
            }


            return View(new ProductsIndexViewModel
            {
                Brands = await _brands.GetAllAsync(),
                Categories = await _categories.GetAllAsync(),
                Products = products
            });
        }

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
                Reviews = await _reviews.GetAllAsync(product.Id)
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

            var success = await _products.Purchase(new OrderDto
            {
                Product = product,
                Customer = new CustomerDto
                {
                    Id = 1,
                    Email = "test@test.com",
                    Name = "Test Esting"
                }
            });

            if(success)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(purchase);
        }
    }
}