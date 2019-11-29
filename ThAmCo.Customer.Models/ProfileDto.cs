using System;
using System.Collections.Generic;
using System.Text;

namespace ThAmCo.Customer.Models
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public string DeliverAddress1 { get; set; }
        public string DeliverAddress2 { get; set; }
        public string DeliverAddress3 { get; set; }
        public string Postcode { get; set; }
    }
}
