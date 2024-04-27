using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace auth_mvc_client.httpservice
{
    public class HttpService(HttpClient httpClient) : IHttpService
    {
        public async Task<IEnumerable<WeatherForecastApiModel>> GetWeatherForecasts(string jwtToken)
        {
            // Replace with logic to retrieve token from storage (e.g., cookie)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await httpClient.GetAsync("https://localhost:7101/api/weather");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WeatherForecastApiModel[]>();
                // Handle successful response with content
                return content ?? [];
            }
            return Array.Empty<WeatherForecastApiModel>();
        }

        public Task<string> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
