using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Web.Models
{
    public class ProductsDetailsViewModel
    {
        public ProductDto Product { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
