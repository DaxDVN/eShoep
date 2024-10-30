using System.Security.Cryptography;
using Identity.API.Data;
using Identity.API.Entities;
using Identity.API.Extensions;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>(opts =>
    {
        opts.Password.RequireDigit = false;
        opts.Password.RequireLowercase = false; 
        opts.Password.RequireUppercase = false;
        opts.Password.RequireNonAlphanumeric = true;
        opts.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();

    await IdentityDataInitializer.SeedData(app.Services);
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.MapPost("/api/auth/login",
    async (LoginRequest loginRequest, UserManager<User> userManager, TokenService tokenService) =>
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            return Results.Unauthorized();
        }

        var token = await tokenService.GenerateTokenAsync(user);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return Results.Ok(new
        {
            tokenType = "Bearer",
            accessToken = token,
            expiresIn = 3600,
            refreshToken
        });
    });
app.MapPost("/api/auth/register", async (UserRegisterRequest registerRequest, UserManager<User> userManager) =>
{
    var user = new User
    {
        FirstName = registerRequest.FirstName,
        LastName = registerRequest.LastName,
        Email = registerRequest.Email,
        PhoneNumber = registerRequest.PhoneNumber,
        UserName = registerRequest.Email
    };

    var result = await userManager.CreateAsync(user, registerRequest.Password);

    return result.Succeeded
        ? Results.Ok(new { message = "Registration successful." })
        : Results.BadRequest(new { errors = result.Errors.Select(e => e.Description) });
});
app.MapControllers();

app.Run();

public record UserRegisterRequest(string FirstName, string LastName, string Email, string PhoneNumber, string Password);