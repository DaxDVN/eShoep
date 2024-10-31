using Carter;
using Common.Behaviors;
using Common.Exceptions;
using Discount.API.Data;
using Discount.API.Services;
using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssembly(assembly);
  config.AddOpenBehavior(typeof(ValidationBehavior<,>));
  config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts => { opts.Connection(builder.Configuration.GetConnectionString("Database")!); })
    .UseLightweightSessions();

if (builder.Environment.IsDevelopment())
  builder.Services.InitializeMartenWith<DiscountInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();
app.MapGrpcService<DiscountService>();

app.UseExceptionHandler(options => { });

app.Run();