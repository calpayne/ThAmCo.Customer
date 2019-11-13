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
        Task<IEnumerable<ProductDto>> GetAllAsync(List<int> brands, List<int> categories, string term, double? minPrice, double? maxPrice);
        Task<ProductDto> GetByIDAsync(int id);
    }
}
