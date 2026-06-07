using AuthApi.Data;
using AuthApi.DTOs;
using AuthApi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        this._context = context;
    }
    
    // método para registrar um usuário no banco.
    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        // verifica se o e-mail ja existe. caso positivo lança exceção.
        var exists = await _context.Users
            .AnyAsync(u => u.Email == request.Email);

        if (exists)
        {
            throw new Exception("email already registered.");
        }

        // encriptografa a senha do request.
        var encryptedPass = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            request.Name,
            request.Email,
            encryptedPass);
        
        // salvar o novo usuario
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new AuthResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString()
        );
    }
        
    
}