using System.ComponentModel.DataAnnotations;

namespace auth_server.Models;

public record RegisterModel
{
    [Required]
    [MaxLength(16)]
    [MinLength(8)]
    public string? Username { get; init; }

    [Required]
    [MaxLength(16)]
    [DataType(DataType.Password)]
    public string? Password { get; init; }

    [Required]
    [MaxLength(16)]
    public string? ConfirmPassword { get; init; }
}