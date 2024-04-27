namespace auth_client.service.Models;

public class WeatherForecastModel
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
    public string Day => Date.DayOfWeek.ToString();
    public string Month => Date.Month.ToString();
}