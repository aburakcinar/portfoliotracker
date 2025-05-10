
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.BankAccount.WebApi.Requests;
using PortfolioTracker.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Placeholder>());


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

var mapGroup = app.MapGroup("/api/bankaccount");

mapGroup.MapCreateBankAccount();
mapGroup.MapGetBankAccount();
mapGroup.MapListBankAccounts();
mapGroup.MapDeleteBankAccount();
mapGroup.MapImportBankAccounts();

mapGroup.MapGet("/health", () => Results.Ok(@"BankAccount WebApi"));

// Run
app.Run();

class Placeholder { }
