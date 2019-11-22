using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Products
{
    class ProductsService : IProductsService
    {
        private readonly HttpClient _client;

        public ProductsService(IConfiguration config, HttpClient client)
        {
            client.BaseAddress = new System.Uri(config.GetConnectionString("Products"));
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _client = client;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            IEnumerable<ProductDto> products;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/products");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }
            catch (HttpRequestException)
            {
                products = Array.Empty<ProductDto>();
            }

            return products;
        }

        private string AddArrayToQuery(int[] array, string paramter)
        {
            String query = "";

            if (array != null && array.Length != 0)
            {
                foreach (int item in array)
                {
                    query += "&" + paramter + "= " + item;
                }
            }

            return query;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(int[] brands, int[] categories, string term, double? minPrice, double? maxPrice)
        {
            IEnumerable<ProductDto> products;
            string searchQuery = "/" + AddArrayToQuery(brands, "brands") + AddArrayToQuery(categories, "categories") + 
                                (term != null ? "&term=" + term : "") +
                                (minPrice != null ? "&minPrice=" + minPrice : "") +
                                (maxPrice != null ? "&maxPrice=" + maxPrice : "")
                                .Replace("/&", "/?");

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/products" + searchQuery);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
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
                HttpResponseMessage response = await _client.GetAsync("/api/products/" + id);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                product = await response.Content.ReadAsAsync<ProductDto>();
            }
            catch (HttpRequestException)
            {
                product = null;
            }

            return product;
        }
    }
}
