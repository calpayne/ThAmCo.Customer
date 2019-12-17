using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Orders
{
    public interface IOrdersService
    {
        Task<bool> CustomerHasOrderedAsync(int productId, string customerId);
        Task<IEnumerable<OrderGetDto>> GetOrdersAsync(string customerId);
        Task<bool> Purchase(OrderDto order);
    }
}
