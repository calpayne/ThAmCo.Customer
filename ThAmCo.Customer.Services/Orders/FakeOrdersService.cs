using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Orders
{
    public class FakeOrdersService : IOrdersService
    {
        private IEnumerable<OrderGetDto> _orders;

        public FakeOrdersService()
        {
            _orders = new List<OrderGetDto>
            {
                new OrderGetDto { Id = 1, TimePlaced = new DateTime(2019, 12, 7), Price = 10.25, TimeDispatched = new DateTime(2019, 12, 8), Product = new ProductDto { Id = 1, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 }, CustomerId = "f32d935b-f175-4450-a93e-e48711c4d481" },
                new OrderGetDto { Id = 2, TimePlaced = new DateTime(2019, 12, 9), Price = 50.25, TimeDispatched = null, Product = new ProductDto { Id = 2, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", Name = "Chocolate Cover", Price = 50.25, StockLevel = 12 }, CustomerId = "f32d935b-f175-4450-a93e-e48711c4d481" },
                new OrderGetDto { Id = 3, TimePlaced = new DateTime(2019, 12, 12), Price = 100.25, TimeDispatched = null, Product = new ProductDto { Id = 3, Currency = "£", BrandId = 3, CategoryId = 2, Description = "Lamely adapted used and dirty teatowel.  Guaranteed fewer than two holes.", Name = "Cloth Cover", Price = 100.25, StockLevel = 24 }, CustomerId = "f32d935b-f175-4450-a93e-e48711c4d4812" }
            };
        }

        public Task<bool> CustomerHasOrderedAsync(int productId, string customerId)
        {
            var result = _orders.Where(o => o.CustomerId == customerId && o.Product.Id == productId);
            return Task.FromResult(result.Count() != 0);
        }

        public Task<IEnumerable<OrderGetDto>> GetOrdersAsync(string customerId)
        {
            return Task.FromResult(_orders.Where(o => o.CustomerId == customerId));
        }
    }
}
