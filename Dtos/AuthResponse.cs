namespace AuthApi.DTOs;

public record AuthResponse
(
    Guid Id,
    string Name,
    string Email,
    string Role
);