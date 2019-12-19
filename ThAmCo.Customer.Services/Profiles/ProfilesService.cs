using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Profiles
{
    public class ProfilesService : IProfilesService
    {
        private readonly HttpClient _client;

        public ProfilesService(HttpClient client)
        {
            _client = client;
        }

        public Task<bool> CanPurchase(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileDto> GetProfileAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProfileAsync(ProfileDto profile)
        {
            throw new NotImplementedException();
        }
    }
}
