using System;
using System.Collections.Generic;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class OrderDto
    {
        public ProductDto Product { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
