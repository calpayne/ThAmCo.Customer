using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Reviews
{
    public class ReviewsService : IReviewsService
    {
        private readonly HttpClient _client;

        public ReviewsService(IConfiguration config, HttpClient client)
        {
            client.BaseAddress = new System.Uri(config.GetConnectionString("Reviews"));
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _client = client;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync(int id)
        {
            IEnumerable <ReviewDto> review;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/reviews/" + id);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                review = await response.Content.ReadAsAsync< IEnumerable<ReviewDto>>();
            }
            catch (HttpRequestException)
            {
                review = Array.Empty<ReviewDto>();
            }

            return review;
        }
    }
}
