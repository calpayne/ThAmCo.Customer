using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Brands;
using ThAmCo.Customer.Services.Categories;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _products;
        private readonly IBrandsService _brands;
        private readonly ICategoriesService _categories;

        public ProductsController(IProductsService products, IBrandsService brands, ICategoriesService categories)
        {
            _products = products;
            _brands = brands;
            _categories = categories;
        }

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

            return View(product);
        }
    }
}