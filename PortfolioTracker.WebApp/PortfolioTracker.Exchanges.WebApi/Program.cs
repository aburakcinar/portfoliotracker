using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Exchanges.WebApi.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Placeholder>());

builder.Services
    .AddDbContext<PortfolioContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("portfoliodb"))
    );

builder.Services.AddTransient<IPortfolioContext>(x => x.GetRequiredService<PortfolioContext>());

// Build
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var mapGroup = app.MapGroup("/api/exchange");
mapGroup.MapSearchExhanges();
mapGroup.MapGetExchange();

mapGroup.MapGet("/health", () => Results.Ok(@"Exchange WebApi"));

// Run
app.Run();

class Placeholder { }
