using System;
using System.Collections.Generic;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class OrderGetDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
        public double Price { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
    }
}
