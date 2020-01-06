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

namespace ThAmCo.Customer.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _auth;

        public ProductsService(HttpClient client, IAuthService auth)
        {
            _client = client;
            _auth = auth;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            IEnumerable<ProductDto> products;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync("/api/products");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }
            catch (SocketException)
            {
                products = Array.Empty<ProductDto>();
            }
            catch (BrokenCircuitException)
            {
                products = Array.Empty<ProductDto>();
            }
            catch (HttpRequestException)
            {
                products = Array.Empty<ProductDto>();
            }

            return products;
        }

        private string AddArrayToQuery(int[] array, string paramter)
        {
            string query = "";

            if (array != null && array.Length != 0)
            {
                foreach (int item in array)
                {
                    query += "&" + paramter + "=" + item;
                }
            }

            return query;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(int[] brands, int[] categories, string term, double? minPrice, double? maxPrice)
        {
            IEnumerable<ProductDto> products;
            string apiString = "/api/products/" + AddArrayToQuery(brands, "brands") + AddArrayToQuery(categories, "categories") +
                                (term != null ? "&term=" + term : "") +
                                (minPrice != null ? "&minPrice=" + minPrice : "") +
                                (maxPrice != null ? "&maxPrice=" + maxPrice : "");

            apiString = apiString.Replace("/api/products/&", "/api/products/?");

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync(apiString);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }
            catch (SocketException)
            {
                products = Array.Empty<ProductDto>();
            }
            catch (BrokenCircuitException)
            {
                products = Array.Empty<ProductDto>();
            }
            catch (HttpRequestException)
            {
                products = Array.Empty<ProductDto>();
            }

            return products;
        }

        public async Task<ProductDto> GetByIDAsync(int id)
        {
            ProductDto product;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.GetAsync("/api/products/" + id);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                product = await response.Content.ReadAsAsync<ProductDto>();
            }
            catch (SocketException)
            {
                product = null;
            }
            catch (BrokenCircuitException)
            {
                product = null;
            }
            catch (HttpRequestException)
            {
                product = null;
            }

            return product;
        }

        public async Task<bool> PurchaseAsync(OrderDto order)
        {
            bool has = false;
            OrderDto result;

            try
            {
                _client.SetBearerToken(await _auth.GetProductsToken());
                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/products/purchase/", order);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadAsAsync<OrderDto>();

                has = result.Customer.Id == order.Customer.Id && result.Product.Id == order.Product.Id;
            }
            catch (SocketException)
            {
                result = null;
            }
            catch (BrokenCircuitException)
            {
                result = null;
            }
            catch (HttpRequestException)
            {
                result = null;
            }

            return has;
        }
    }
}
