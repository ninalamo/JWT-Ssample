namespace auth_client.domain;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

public interface IWeatherRepository
{
    IEnumerable<WeatherForecast> Get();
}