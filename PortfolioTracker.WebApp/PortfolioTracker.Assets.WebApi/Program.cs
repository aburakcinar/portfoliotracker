using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Assets.WebApp.Requests;
using PortfolioTracker.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
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

var mapGroup = app.MapGroup("/api/asset");

mapGroup.MapSearchAsset();
mapGroup.MapGetAsset();
mapGroup.MapListAssetsSummary();
mapGroup.MapGet("/health", () => Results.Ok(@"Asset WebApi"));

app.Run();

class Placeholder { }
