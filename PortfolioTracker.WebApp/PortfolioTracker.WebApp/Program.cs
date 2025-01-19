using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("Postgres");

builder.Services
    .AddDbContext<PortfolioContext>(options => options.UseNpgsql(connStr));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PortfolioContext>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRemixApp",
        policy => policy.WithOrigins("http://localhost:5173") // Adjust as needed
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddSingleton<IStockRepository, StockRepository>();
builder.Services.AddTransient<ILocaleImporter, LocaleImporter>();
builder.Services.AddTransient<IExchangeImporter, ExchangeImporter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStockRepository();

//app.UseAuthorization();

app.MapControllers();

app.Run();