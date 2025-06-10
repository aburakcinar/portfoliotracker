using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Portfolio.WebApi.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// Register DbContext
builder.Services.AddDbContext<PortfolioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("portfoliodb")));
builder.Services.AddScoped<IPortfolioContext, PortfolioContext>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Group endpoints
var mapGroup = app.MapGroup(@"/api/portfolio");
mapGroup.MapListPortfoliosByBankAccountId();
mapGroup.MapListAllPortfolios();
mapGroup.MapGetPortfolioById();
mapGroup.MapCreatePortfolio();
mapGroup.MapUpdatePortfolio();
mapGroup.MapDeletePortfolio();

mapGroup.MapGet(@"/health", () => Results.Ok(@"Exchange WebApi"));

app.Run();

// Placeholder class to assist with MediatR assembly scanning
internal class Placeholder { }
