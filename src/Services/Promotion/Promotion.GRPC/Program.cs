using Marten;
using Promotion.Application;
using Promotion.GRPC.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
});

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddMarten(opts => { opts.Connection(builder.Configuration.GetConnectionString("Database")!); })
    .UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CouponService>();
app.Run();