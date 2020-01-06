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

namespace ThAmCo.Customer.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _auth;

        public OrdersService(HttpClient client, IAuthService auth)
        {
            _client = client;
            _auth = auth;
        }

        public async Task<bool> CustomerHasOrderedAsync(int productId, string customerId)
        {
            bool has;

            try
            {
                _client.SetBearerToken(await _auth.GetOrdersToken());
                HttpResponseMessage response = await _client.GetAsync("/api/orders/hasordered/?productId=" + productId + "&customerId=" + customerId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                response.EnsureSuccessStatusCode();

                has = await response.Content.ReadAsAsync<bool>();
            }
            catch (SocketException)
            {
                has = false;
            }
            catch (BrokenCircuitException)
            {
                has = false;
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
                _client.SetBearerToken(await _auth.GetOrdersToken());
                HttpResponseMessage response = await _client.GetAsync("/api/orders/" + customerId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                orders = await response.Content.ReadAsAsync<IEnumerable<OrderGetDto>>();
            }
            catch (SocketException)
            {
                orders = Array.Empty<OrderGetDto>();
            }
            catch (BrokenCircuitException)
            {
                orders = Array.Empty<OrderGetDto>();
            }
            catch (HttpRequestException)
            {
                orders = Array.Empty<OrderGetDto>();
            }

            return orders;
        }
    }
}
