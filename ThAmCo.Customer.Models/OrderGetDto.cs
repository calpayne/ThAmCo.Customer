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
        public DateTime TimePlaced { get; set; }
        public DateTime? TimeDispatched { get; set; }
        public string CustomerId { get; set; }
        public string ViewStatus { get
            {
                return TimeDispatched == null ? "Not dispatched" : TimeDispatched.ToString();
            }
        }
    }
}
