using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class ReviewDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
