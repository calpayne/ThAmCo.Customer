using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using System.Linq;

namespace ThAmCo.Customer.Services.Products
{
    public class FakeProductsService : IProductsService
    {
        private readonly IEnumerable<ProductDto> _products;

        public FakeProductsService()
        {
            _products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 },
                new ProductDto { Id = 2, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", Name = "Chocolate Cover", Price = 50.25, StockLevel = 12 },
                new ProductDto { Id = 3, Currency = "£",BrandId = 3, CategoryId = 2, Description = "Lamely adapted used and dirty teatowel.  Guaranteed fewer than two holes.", Name = "Cloth Cover", Price = 100.25, StockLevel = 24 },
                new ProductDto { Id = 4, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Especially toughen and harden sponge entirely encases your device to prevent any interaction.", Name = "Harden Sponge Case", Price = 60.25, StockLevel = 4 },
                new ProductDto { Id = 5, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", Name = "Water Bath Case", Price = 20.25, StockLevel = 5 },
                new ProductDto { Id = 6, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only).", Name = "Smartphone Car Holder", Price = 200.25, StockLevel = 13 },
                new ProductDto { Id = 7, Currency = "£", BrandId = 3, CategoryId = 2, Description = "Keep your device on your arm with this general purpose sticky tape.", Name = "Sticky Tape Sport Armband", Price = 20.25, StockLevel = 18 },
                new ProductDto { Id = 8, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Stengthen HB pencils guaranteed to leave a mark.", Name = "Real Pencil Stylus", Price = 35.25, StockLevel = 14 },
                new ProductDto { Id = 9, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Coat your mobile device screen in a scratch resistant, opaque film.", Name = "Spray Paint Screen Protector", Price = 45.25, StockLevel = 8 },
                new ProductDto { Id = 10, Currency = "£", BrandId = 2, CategoryId = 3, Description = "For his or her sensory pleasure. Fits few known smartphones.", Name = "Rippled Screen Protector", Price = 85.25, StockLevel = 5 },
                new ProductDto { Id = 11, Currency = "£", BrandId = 3, CategoryId = 2, Description = "For an odour than lingers on your device.", Name = "Fish Scented Screen Protector", Price = 125.25, StockLevel = 3 },
                new ProductDto { Id = 12, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Guaranteed not to conduct electical charge from your fingers.", Name = "Non-conductive Screen Protector", Price = 99.25, StockLevel = 0 }
            };
        }

        public Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return Task.FromResult(_products);
        }

        public Task<IEnumerable<ProductDto>> GetAllAsync(int[] brands, int[] categories, string term, double? minPrice, double? maxPrice)
        {
            return Task.FromResult(_products.Where(p => term == null || (p.Name.ToLower().Contains(term.ToLower()) || p.Description.ToLower().Contains(term.ToLower())))
                                            .Where(p => brands.Count() == 0 || brands.Contains(p.BrandId))
                                            .Where(p => categories.Count() == 0 || categories.Contains(p.CategoryId))
                                            .Where(p => minPrice == null || p.Price >= minPrice)
                                            .Where(p => maxPrice == null || p.Price <= maxPrice));
        }

        public Task<ProductDto> GetByIDAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public Task<bool> Purchase(OrderDto order)
        {
            return Task.FromResult(true);
        }
    }
}
