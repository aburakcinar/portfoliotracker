using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Requests;
using PortfolioTracker.Imports.WebApi.Services;
using PortfolioTracker.Imports.WebApi.Services.Handlers;
using PortfolioTracker.WebApp.Services.TransactionsImporter;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Placeholder>());

builder.Services.AddTransient<ITransactionImportExtension, ScalableCapitalCvsFileTransactionImportExtension>();
builder.Services.AddTransient<ITransactionTypeHandler, AccountFeeTransactionHandler>();
builder.Services.AddTransient<ITransactionTypeHandler, BuyAssetTransactionHandler>();
builder.Services.AddTransient<ITransactionTypeHandler, DepositTransactionHandler>();
builder.Services.AddTransient<ITransactionTypeHandler, DividendDistributionTransactionHandler>();
builder.Services.AddTransient<ITransactionTypeHandler, InterestTransactionHandler>();
builder.Services.AddTransient<ITransactionTypeHandler, SellAssetTransactionHandler>();
builder.Services.AddTransient<ITransactionTypeHandler, WithdrawTransactionHandler>();
builder.Services.AddTransient<ITransactionsImporter, TransactionsImporter>();


builder.Services
    .AddDbContext<PortfolioContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("portfoliodb"))
    );

builder.Services.AddTransient<IPortfolioContext>(x => x.GetRequiredService<PortfolioContext>());

// Build
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var mapGroup = app.MapGroup("/api/import");

mapGroup.MapImportTransactionsFromFile();
mapGroup.MapDraftImportTransactionsFromFile();

mapGroup.MapGet("/health", () => Results.Ok(@"Imports WebApi"));

// Run
app.Run();

class Placeholder { }
