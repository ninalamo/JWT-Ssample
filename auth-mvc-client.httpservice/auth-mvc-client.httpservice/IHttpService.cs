using System.Net.Http;

namespace auth_mvc_client.httpservice
{
    public interface IHttpService
    {
        Task<IEnumerable<WeatherForecastApiModel>> GetWeatherForecasts(string jwtToken);
    }
}
