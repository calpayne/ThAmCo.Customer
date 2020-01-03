using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required, EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
