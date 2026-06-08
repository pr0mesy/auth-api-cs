using AuthApi.Data;
using AuthApi.DTOs;
using AuthApi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;
    
    public AuthService(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email);

        if (userExists)
        {
            throw new Exception("email already registered.");
        }

        var encryptedPass = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            request.Name,
            request.Email,
            encryptedPass);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var token = _tokenService.GenerateToken(user);

        return new AuthResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token
        );
    }
    
    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == request.Email
            );

        if (user is null)
        {
            throw new Exception("invalid credentials.");
        }

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );
        
        if (!validPassword)
        {
            throw new Exception("invalid credentials.");
        }
        
        var token = _tokenService.GenerateToken(user);
        
        return new AuthResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token
        );
    }
        
    
}