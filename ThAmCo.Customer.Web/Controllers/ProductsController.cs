using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Products;

namespace ThAmCo.Customer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _products;

        public ProductsController(IProductsService products)
        {
            _products = products;
        }

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = await _products.GetAllAsync();

            if (products == null)
            {
                products = Array.Empty<ProductDto>();
            }

            return View(products.ToList());
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