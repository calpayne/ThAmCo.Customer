using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _client;

        public CategoriesService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            IEnumerable<CategoryDto> categories;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/categories");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                categories = await response.Content.ReadAsAsync<IEnumerable<CategoryDto>>();
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
                HttpResponseMessage response = await _client.GetAsync("/api/categories/" + id);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                category = await response.Content.ReadAsAsync<CategoryDto>();
            }
            catch (HttpRequestException)
            {
                category = null;
            }

            return category;
        }
    }
}
