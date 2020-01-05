using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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

        public async Task<bool> CanPurchase(string customerId)
        {
            ProfileDto profile;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/profiles/canpurchase/" + customerId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                profile = await response.Content.ReadAsAsync<ProfileDto>();
            }
            catch (SocketException)
            {
                profile = null;
            }
            catch (BrokenCircuitException)
            {
                profile = null;
            }
            catch (HttpRequestException)
            {
                profile = null;
            }

            return profile != null && profile.Id == customerId;
        }

        public async Task<ProfileDto> GetProfileAsync(string id)
        {
            ProfileDto profile;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/profiles/" + id);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                profile = await response.Content.ReadAsAsync<ProfileDto>();
            }
            catch (SocketException)
            {
                profile = null;
            }
            catch (BrokenCircuitException)
            {
                profile = null;
            }
            catch (HttpRequestException)
            {
                profile = null;
            }

            return profile;
        }

        public async Task<bool> RequestDeletion(string customerId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/profile/delreq/" + customerId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                ProfileDto data = await response.Content.ReadAsAsync<ProfileDto>();

                if (data == null || data.Id != customerId)
                {
                    return false;
                }
            }
            catch (SocketException)
            {
                return false;
            }
            catch (BrokenCircuitException)
            {
                return false;
            }
            catch (HttpRequestException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateProfileAsync(ProfileDto profile)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/profiles/", profile);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                ProfileDto data = await response.Content.ReadAsAsync<ProfileDto>();

                if (data == null || data.Id != profile.Id)
                {
                    return false;
                }
            }
            catch (SocketException)
            {
                return false;
            }
            catch (BrokenCircuitException)
            {
                return false;
            }
            catch (HttpRequestException)
            {
                return false;
            }

            return true;
        }
    }
}
