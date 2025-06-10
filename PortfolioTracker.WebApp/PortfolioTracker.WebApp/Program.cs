using FinanceData.Business;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Services;
using PortfolioTracker.WebApp.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStockRepository, StockRepository>();
builder.Services.AddTransient<IAssetService, AssetService>();
builder.Services.AddTransient<IPortfolioImportService, PortfolioImportService>();
builder.Services.AddTransient<IDbSeedService, DbSeedService>();

builder.Services
    .AddDbContext<PortfolioContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("portfoliodb"))
    );

builder.Services.AddTransient<IPortfolioContext>(x => x.GetRequiredService<PortfolioContext>());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterPlaceholder>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .AllowAnyOrigin() // Adjust as needed
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.AddFinanceDataBusiness();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

