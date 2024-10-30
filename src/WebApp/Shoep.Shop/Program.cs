using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Refit;
using Shoep.Shop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

// Add Cookie-based authentication for Razor Pages login sessions
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration["ApiSettings:IdentityApiUrl"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["ApiSettings:IdentityApiUrl"],
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true
        };
    });

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); });
builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); });
builder.Services.AddRefitClient<IIdentityService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:IdentityApiUrl"]!); });

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();