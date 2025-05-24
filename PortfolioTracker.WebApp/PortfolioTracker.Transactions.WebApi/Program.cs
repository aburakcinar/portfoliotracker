using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Transactions.WebApi.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services
    .AddDbContext<PortfolioContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString(@"portfoliodb"))
    );

builder.Services.AddTransient<IPortfolioContext>(x => x.GetRequiredService<PortfolioContext>());

// Build
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API group mapping for transactions
var mapGroup = app.MapGroup(@"/api/transaction");
mapGroup.MapListBankAccountTransactionsEndpoint();
mapGroup.MapAddTransactionEndpoint();
mapGroup.MapListTransactionActionTypesEndpoint();

mapGroup.MapGet(@"/health", () => Results.Ok(@"Transactions WebApi"));

app.Run();
