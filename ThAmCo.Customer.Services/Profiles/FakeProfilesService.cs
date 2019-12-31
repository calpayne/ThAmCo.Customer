using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Profiles
{
    public class FakeProfilesService : IProfilesService
    {
        private IEnumerable<ProfileDto> _profiles;

        public FakeProfilesService()
        {
            _profiles = new List<ProfileDto>
            {
                new ProfileDto { Id = "f32d935b-f175-4450-a93e-e48711c4d481", DeliverAddress1 = "DeliverAddress1", DeliverAddress2 = "DeliverAddress2", DeliverAddress3 = "DeliverAddress3", Email = "email@email.com", FirstName = "First", Postcode = "Postcode", Surname = "Sur", TelNo = "1234" }
            };
        }

        public Task<bool> CanPurchase(string customerId)
        {
            return Task.FromResult(customerId != "not-allowed-to-purchase");
        }

        public Task<ProfileDto> GetProfileAsync(string id)
        {
            return Task.FromResult(_profiles.FirstOrDefault(p => p.Id == id));
        }

        public Task<bool> RequestDeletion(string customerId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> UpdateProfileAsync(ProfileDto profile)
        {
            var data = _profiles.FirstOrDefault(p => p.Id == profile.Id);

            if (data == null)
            {
                return Task.FromResult(false);
            }

            data = profile;

            return Task.FromResult(true);
        }
    }
}
