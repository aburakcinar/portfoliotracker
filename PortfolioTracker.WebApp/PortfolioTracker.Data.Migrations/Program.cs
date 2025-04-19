

using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Migrations;
using PortfolioTracker.Data.Migrations.Services;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApiDbInitializer>());
builder.Services.AddTransient<IExchangeReader, CvsExchangeReader>();
builder.Services.AddTransient<ILocalesReader, CvsLocalesReader>();
builder.Services.AddTransient<ISeedService, SeedService>();

builder.Services.AddDbContextPool<PortfolioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("portfoliodb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("PortfolioTracker.Data.Migrations");
    }));

builder.Services.AddTransient<IPortfolioContext>(sr  => sr.GetRequiredService<PortfolioContext>());

builder.Services.AddHostedService<ApiDbInitializer>();

builder.AddServiceDefaults();


var app = builder.Build();

app.Run();

