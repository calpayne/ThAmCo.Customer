using IdentityModel.Client;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Auth;

namespace ThAmCo.Customer.Services.Brands
{
    public class BrandsService : IBrandsService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _auth;

        public BrandsService(HttpClient client, IAuthService auth)
        {
            _client = client;
            _auth = auth;
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            IEnumerable<BrandDto> brands;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync("/api/brands");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                brands = await response.Content.ReadAsAsync<IEnumerable<BrandDto>>();
            }
            catch (SocketException)
            {
                brands = Array.Empty<BrandDto>();
            }
            catch (BrokenCircuitException)
            {
                brands = Array.Empty<BrandDto>();
            }
            catch (HttpRequestException)
            {
                brands = Array.Empty<BrandDto>();
            }

            return brands;
        }

        public async Task<BrandDto> GetByIDAsync(int id)
        {
            BrandDto brand;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync("/api/brands/" + id);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                brand = await response.Content.ReadAsAsync<BrandDto>();
            }
            catch (SocketException)
            {
                brand = null;
            }
            catch (BrokenCircuitException)
            {
                brand = null;
            }
            catch (HttpRequestException)
            {
                brand = null;
            }

            return brand;
        }
    }
}
