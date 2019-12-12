using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Orders
{
    public interface IOrdersService
    {
        Task<bool> CustomerHasOrderedAsync(int productId, int customerId);

        Task<IEnumerable<OrderGetDto>> GetOrdersAsync(int customerId);
    }
}
