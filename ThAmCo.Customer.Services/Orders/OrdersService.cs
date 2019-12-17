using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly HttpClient _client;

        public OrdersService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> CustomerHasOrderedAsync(int productId, string customerId)
        {
            bool has;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/orders/hasordered/?productId=" + productId + "&customerId=" + customerId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                has = await response.Content.ReadAsAsync<bool>();
            }
            catch (HttpRequestException)
            {
                has = false;
            }

            return has;
        }

        public async Task<IEnumerable<OrderGetDto>> GetOrdersAsync(string customerId)
        {
            IEnumerable<OrderGetDto> orders;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/orders/" + customerId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                orders = await response.Content.ReadAsAsync<IEnumerable<OrderGetDto>>();
            }
            catch (HttpRequestException)
            {
                orders = Array.Empty<OrderGetDto>();
            }

            return orders;
        }
    }
}
