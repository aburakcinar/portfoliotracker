
using Finance.Data.Migrations;
using Finance.Data.Migrations.Services;
using FinanceData.Business.DataStore;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Service Registration
builder.Services.AddSingleton<ISeedService, SeedService>();


// Database Context
var connStr = builder.Configuration.GetConnectionString("financedb");

builder.Services.AddDbContextPool<FinansDataContext>(options =>
    options.UseNpgsql(connStr, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("Finance.Data.Migrations");
    }));
builder.Services.AddTransient<IFinansDataContext>(sr => sr.GetRequiredService<FinansDataContext>());


// Worker
builder.Services.AddHostedService<AppDbInitializer>();

// Service Defaults
builder.AddServiceDefaults();

// App
var host = builder.Build();
host.Run();
