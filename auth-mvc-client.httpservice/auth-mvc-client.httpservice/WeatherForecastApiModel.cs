namespace auth_mvc_client.httpservice
{
    public record WeatherForecastApiModel(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => TemperatureC * 10;
    }
}
