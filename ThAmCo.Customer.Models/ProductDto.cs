using System;
using System.Collections.Generic;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
        public int StockLevel { get; set; }
    }
}
