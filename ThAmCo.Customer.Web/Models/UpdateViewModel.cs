using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Web.Models
{
    public class UpdateViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string TelNo { get; set; }
        [Required]
        public string DeliverAddress1 { get; set; }
        [Required]
        public string DeliverAddress2 { get; set; }
        [Required]
        public string DeliverAddress3 { get; set; }
        [Required, DataType(DataType.PostalCode)]
        public string Postcode { get; set; }

        public static UpdateViewModel Transform(ProfileDto p)
        {
            return new UpdateViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                Surname = p.Surname,
                Email = p.Email,
                TelNo = p.TelNo,
                DeliverAddress1 = p.DeliverAddress1,
                DeliverAddress2 = p.DeliverAddress2,
                DeliverAddress3 = p.DeliverAddress3,
                Postcode = p.Postcode
            };
        }

        public static ProfileDto ToProfileDto(UpdateViewModel p)
        {
            return new ProfileDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                Surname = p.Surname,
                Email = p.Email,
                TelNo = p.TelNo,
                DeliverAddress1 = p.DeliverAddress1,
                DeliverAddress2 = p.DeliverAddress2,
                DeliverAddress3 = p.DeliverAddress3,
                Postcode = p.Postcode
            };
        }
    }
}
