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

namespace ThAmCo.Customer.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _auth;

        public CategoriesService(HttpClient client, IAuthService auth)
        {
            _client = client;
            _auth = auth;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            IEnumerable<CategoryDto> categories;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync("/api/categories");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                categories = await response.Content.ReadAsAsync<IEnumerable<CategoryDto>>();
            }
            catch (SocketException)
            {
                categories = Array.Empty<CategoryDto>();
            }
            catch (BrokenCircuitException)
            {
                categories = Array.Empty<CategoryDto>();
            }
            catch (HttpRequestException)
            {
                categories = Array.Empty<CategoryDto>();
            }

            return categories;
        }

        public async Task<CategoryDto> GetByIDAsync(int id)
        {
            CategoryDto category;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync("/api/categories/" + id);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                category = await response.Content.ReadAsAsync<CategoryDto>();
            }
            catch (SocketException)
            {
                category = null;
            }
            catch (BrokenCircuitException)
            {
                category = null;
            }
            catch (HttpRequestException)
            {
                category = null;
            }

            return category;
        }
    }
}
