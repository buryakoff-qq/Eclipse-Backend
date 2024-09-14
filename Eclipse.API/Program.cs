using Eclipse.Core;
using Eclipse.Core.DTOs.UserDTOs;
using Eclipse.Core.Interfaces.IPassword;
using Eclipse.Core.Interfaces.IToken;
using Eclipse.Core.Interfaces.IUser;
using Eclipse.Core.Services.Password;
using Eclipse.Core.Services.Token;
using Eclipse.Core.Services.UserService;
using Eclipse.Core.Services.Validation;
using Eclipse.Infrastructure;
using Eclipse.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IValidator<UserRegistration>, RegisterValidation>();
builder.Services.AddScoped<IValidator<UserLogin>, LoginValidation>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddDbContext<EclipseDbContext>();

var configuration = builder.Configuration;

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JwtOptions"));

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JWTOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["Cooooookies"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
Console.WriteLine($"Secret Key: {jwtOptions.SecretKey}");
Console.WriteLine($"Expires Hours: {jwtOptions.ExpiresHours}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
