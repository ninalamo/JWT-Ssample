using System.ComponentModel.DataAnnotations;

namespace auth_mvc_client.Models;

public record LoginViewModel
{
    [Required]
    [MaxLength(16)]
    [MinLength(8)]
    public string? Username { get; init; } = "username";

    [Required]
    [MaxLength(16)]
    [DataType(DataType.Password)]
    public string? Password { get; init; } = "Password1234!";
}
public record WeatherForecastModel(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);