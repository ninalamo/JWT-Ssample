using auth_client.service.Models;

namespace auth_client.service;

public interface IWeatherService
{
    IEnumerable<WeatherForecastModel> GetAll();
}