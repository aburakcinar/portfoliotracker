using FinanceData.Business;
using FinanceData.Business.Api;
using FinanceData.Business.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString(@"TimescaleDb-FinanceData");

// Add Logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddControllers();

//builder.Services.AddDbContext<FinansDataContext>(options => options.UseNpgsql(connectionString));
//builder.Services.AddTransient<IFinansDataContext>(x => x.GetRequiredService<FinansDataContext>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddFinanceDataBusiness();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRemixApp",
        policy => policy
            .AllowAnyOrigin() 
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

app.MapControllers();

app.MapGet(@"api/currency/rate/{from}/to/{to}",
    async (ICurrencyRateService service, string from, string to) =>
    {
        try
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return Results.BadRequest("Currency codes cannot be empty");

            var result = await service.GetRateAsync(new CurrencyRateQueryModel
            { Base = from, Target = to, Date = DateOnly.FromDateTime(DateTime.Now) });

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
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
            try
            {
                if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    return Results.BadRequest("Currency codes cannot be empty");

                var result = await service.GetRateAsync(new CurrencyRateQueryModel
                { Base = from, Target = to, Date = DateOnly.FromDateTime(date) });

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
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

public class CurrencyConversionRequest
{
    public required string FromCurrency { get; set; }
    public required string ToCurrency { get; set; }
    public decimal Amount { get; set; }
}