
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Migrations;
using PortfolioTracker.Data.Migrations.Services;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Services;

var builder = Host.CreateApplicationBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Service Registration
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApiDbInitializer>());
builder.Services.AddTransient<IExchangeReader, CvsExchangeReader>();
builder.Services.AddTransient<ILocalesReader, CvsLocalesReader>();
builder.Services.AddTransient<ISeedService, SeedService>();


// Database Context
var connStr = builder.Configuration.GetConnectionString("portfoliodb");
builder.Services.AddDbContextPool<PortfolioContext>(options =>
    options.UseNpgsql(connStr, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("PortfolioTracker.Data.Migrations");
    }));

builder.Services.AddTransient<IPortfolioContext>(sr  => sr.GetRequiredService<PortfolioContext>());

// Worker
builder.Services.AddHostedService<ApiDbInitializer>();

// Service Defaults
builder.AddServiceDefaults();

// App
var app = builder.Build();
app.Run();

