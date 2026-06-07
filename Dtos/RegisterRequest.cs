using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs;

public record RegisterRequest
(
    [Required]
    [MaxLength(100)]
    string Name,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(6)]
    string Password
);