using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThAmCo.Customer.Services.Orders
{
    public interface IOrdersService
    {
        Task<bool> CustomerHasOrdered(int productId, int customerId);
    }
}
