using auth_mvc_client.httpservice;
using auth_mvc_client.Models;

namespace auth_mvc_client.Extenstions
{
    public static class DtoExtensions
    {
        public static IEnumerable<WeatherForecastModel> ToDto(this IEnumerable<WeatherForecastApiModel> models)
        {
            
            return models
                .Select(x => new WeatherForecastModel(
                x.Date, x.TemperatureC, x.TemperatureF, x.Summary));
            
        }
    }
}
