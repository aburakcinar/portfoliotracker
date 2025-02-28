using FinanceData.Business;
using FinanceData.Business.Api;
using FinanceData.Business.DataStore;
using FinanceData.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(@"TimescaleDb-FinanceData");

builder.Services.AddDbContext<FinansDataContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<IFinansDataContext>(x => x.GetRequiredService<FinansDataContext>());

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBusiness();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet(@"api/currency/rate/{from}/to/{to}",
    async (ICurrencyRateService service, string from, string to) =>
    {
        return await service.GetRateAsync(new CurrencyRateQueryModel
            { Base = from, Target = to, Date = DateOnly.FromDateTime(DateTime.Now) });
    }
)
    .WithName(@"Get Today Currency Rate")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = @"Gets today currency exchange rate for given currency pair",
        Description = @"Returns rate"
    });

app.MapGet(@"api/currency/date/{date}/rate/{from}/to/{to}",
        async (ICurrencyRateService service, string from, string to, DateTime date) =>
        {
            return await service.GetRateAsync(new CurrencyRateQueryModel
                { Base = from, Target = to, Date = DateOnly.FromDateTime(DateTime.Now) });
        }
    )
    .WithName(@"Get Currency Rate by Date")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = @"Gets currency exchange rate for given currency pair by date",
        Description = @"Returns rate"
    });

app.MapPost(@"api/currency/import/ecb", async (IImportBulkService service) => await service.ExecuteAsync());

app.Run();