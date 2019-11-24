using System;
using System.Collections.Generic;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
    }
}
