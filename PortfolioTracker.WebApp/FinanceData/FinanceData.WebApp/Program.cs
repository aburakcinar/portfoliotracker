using FinanceData.Business;
using FinanceData.Business.Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();