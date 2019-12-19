using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly HttpClient _client;

        public ProductsService(HttpClient client)
        {
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
                HttpResponseMessage response = await _client.GetAsync(apiString);
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

        public async Task<bool> PurchaseAsync(OrderDto order)
        {
            bool has = false;
            OrderDto result;

            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/products/purchase", order);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadAsAsync<OrderDto>();

                has = result.Customer.Id == order.Customer.Id && result.Product.Id == order.Product.Id;
            }
            catch (HttpRequestException)
            {
                result = null;
            }

            return has;
        }
    }
}
