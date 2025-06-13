using Microsoft.Extensions.Logging;
using Order.Core.DTO;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersMicroserviceClient> _logger;

        public UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<UserDTO?> GetUserByUserID(int userID)
        {
            try
            {
                //var url = _httpClient.BaseAddress + $"api/ApplicationUsers/{userID}";
                var url = _httpClient.BaseAddress + $"/gateway/ApplicationUsers/{userID}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        return new UserDTO()
                        {
                            PersonName = "Temporarily Unavailable",
                            Email = "Temporarily Unavailable",
                            Gender = "Temporarily Unavailable",
                            UserID = 0
                        };
                    }
                }


                UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();

                if (user == null)
                {
                    throw new ArgumentException("Invalid User ID");
                }

                return user;
            }

            catch (BrokenCircuitException ex)
            {
                _logger.LogError(ex, "Request failed because of circuit breaker is in Open state. Returning dummy data.");

                return new UserDTO() {
                        PersonName="Temporarily Unavailable (circuit breaker)",
                        Email="Temporarily Unavailable (circuit breaker)",
                        Gender="Temporarily Unavailable (circuit breaker)",
                        UserID=0
                };
            }

            catch (TimeoutRejectedException ex)
            {
                _logger.LogError(ex, "Timeout occurred while fetching user data. Returning dummy data");

                return new UserDTO() {
                        PersonName="Temporarily Unavailable (timeout)",
                        Email="Temporarily Unavailable (timeout)",
                        Gender="Temporarily Unavailable (timeout)",
                        UserID=0 };
            }
        }
    }
}
