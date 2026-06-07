using System.ComponentModel.DataAnnotations;
using AuthApi.Models.Enums;

namespace AuthApi.Models.Entity;

public class User
{
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; private set; } = string.Empty;
    
    [Required]
    [MaxLength(60)]
    public string PasswordHash { get; private set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; private set; }
    
    private User()
    {
    }
    
    public User(
        string name,
        string email,
        string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = UserRole.User;
    }
}