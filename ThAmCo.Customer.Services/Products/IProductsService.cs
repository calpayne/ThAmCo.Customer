using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Products
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIDAsync(int id);
    }
}
