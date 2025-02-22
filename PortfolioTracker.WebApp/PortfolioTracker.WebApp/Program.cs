using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("Postgres_V2");

builder.Services.AddSingleton<IStockRepository, StockRepository>();
builder.Services.AddTransient<ILocaleService, LocaleService>();
builder.Services.AddTransient<IExchangeService, ExchangeService>();
builder.Services.AddTransient<IAssetService, AssetService>();
builder.Services.AddTransient<IPortfolioImportService, PortfolioImportService>();
builder.Services.AddTransient<IDbSeedService, DbSeedService>();

builder.Services
    .AddDbContext<PortfolioContext>(options => options
        .UseNpgsql(connStr)
    );

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PortfolioContext>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRemixApp",
        policy => policy.WithOrigins("http://localhost:5173") // Adjust as needed
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStockRepository();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortfolioContext>();
    context.Database.EnsureCreated();
    
    var seeder = scope.ServiceProvider.GetRequiredService<IDbSeedService>();
    
    await seeder.SeedAsync();
}

app.Run();