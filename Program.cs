using AuthApi.Data;
using AuthApi.Services;
using Microsoft.EntityFrameworkCore;
using AuthApi.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database
builder.Services.AddDbContext<AppDbContext>(
    options =>
        options.UseNpgsql(
            $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
            $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
            $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
            $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
            $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}"
        )
);


// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();


var jwtSettings = new JwtSettings
{
    Key = Environment.GetEnvironmentVariable("JWT_KEY")
        ?? throw new Exception("JWT_KEY not configured"),

    ExpirationMinutes = int.Parse(
        Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES")
        ?? throw new Exception("JWT_EXPIRATION_MINUTES not configured")
    )
};


builder.Services.AddSingleton(jwtSettings);


builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
)
.AddJwtBearer(options =>
{
    var key = Environment.GetEnvironmentVariable("JWT_KEY")
              ?? throw new Exception("JWT_KEY not configured");


    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)
                ),

            ValidateIssuer = false,
            ValidateAudience = false,

            ClockSkew = TimeSpan.Zero
        };
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();


app.Run();