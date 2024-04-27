namespace auth_mvc_client.httpservice
{
    public class FakeWeatherHttpService : IHttpService
    {
        public async Task<IEnumerable<WeatherForecastApiModel>> GetWeatherForecasts(string jwtToken)
        {
            return Get();
        }

        private readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public IEnumerable<WeatherForecastApiModel> Get()
        {
            return Enumerable.Range(1, 5)
                .Select(index =>
                new WeatherForecastApiModel(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),  
                    Random.Shared.Next(-20, 55), 
                    Summaries[Random.Shared.Next(Summaries.Length)]
            )).ToArray();
        }
    }
}
