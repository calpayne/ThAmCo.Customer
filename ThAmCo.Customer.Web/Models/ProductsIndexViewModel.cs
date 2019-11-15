using System;
using System.Collections.Generic;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Web.Models
{
    public class ProductsIndexViewModel
    {
        public IEnumerable<BrandDto> Brands { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
