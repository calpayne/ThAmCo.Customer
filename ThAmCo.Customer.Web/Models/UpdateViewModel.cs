using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Customer.Web.Models
{
    public class UpdateViewModel
    {
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
