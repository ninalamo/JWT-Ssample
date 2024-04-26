using auth_client.domain;
using auth_client.service.Models;

namespace auth_client.service;

public class WeatherService(IWeatherRepository repository) : IWeatherService
{
    
    public IEnumerable<WeatherForecastModel> GetAll()
    {
        return repository.Get().Select(c => new WeatherForecastModel()
        {
            Date = c.Date,
            TemperatureC = c.TemperatureC,
            Summary = c.Summary
        });
    }
}