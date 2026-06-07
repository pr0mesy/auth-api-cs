using AuthApi.Models.Enums;

namespace AuthApi.Models.Entity;

public class User
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; } = UserRole.User;


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
    }
}