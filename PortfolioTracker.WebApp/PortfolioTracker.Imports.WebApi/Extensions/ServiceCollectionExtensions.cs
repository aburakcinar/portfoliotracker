using Microsoft.Extensions.DependencyInjection;
using PortfolioTracker.Imports.WebApi.Services;
using PortfolioTracker.Imports.WebApi.Services.Handlers;

namespace PortfolioTracker.Imports.WebApi.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register import services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all transaction import services
    /// </summary>
    public static IServiceCollection AddTransactionImportServices(this IServiceCollection services)
    {
        // Register main importer
        services.AddScoped<ITransactionsImporter, TransactionsImporter>();
        
        // Register all transaction type handlers
        services.AddScoped<ITransactionTypeHandler, DepositTransactionHandler>();
        services.AddScoped<ITransactionTypeHandler, WithdrawTransactionHandler>();
        services.AddScoped<ITransactionTypeHandler, DividendDistributionTransactionHandler>();
        services.AddScoped<ITransactionTypeHandler, AccountFeeTransactionHandler>();
        services.AddScoped<ITransactionTypeHandler, InterestTransactionHandler>();
        services.AddScoped<ITransactionTypeHandler, BuyAssetTransactionHandler>();
        services.AddScoped<ITransactionTypeHandler, SellAssetTransactionHandler>();
        
        return services;
    }
}
