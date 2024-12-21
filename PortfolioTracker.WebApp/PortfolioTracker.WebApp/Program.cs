using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Portfolio") ?? "Data Source=Portfolio.db";
builder.Services
    .AddDbContext<PortfolioContext>(options => 
        options.UseSqlite(connectionString));

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

//app.UseAuthorization();

app.MapControllers();

app.Run();