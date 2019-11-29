using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThAmCo.Customer.Services.Orders
{
    public class FakeOrdersService : IOrdersService
    {
        public Task<bool> CustomerHasOrdered(int productId, int customerId)
        {
            retrn true;
        }
    }
}
