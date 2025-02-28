using FinanceData.Business.Api;
using FinanceData.Business.Services;
using FinanceData.Business.Services.ExchangeRateProviders;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceData.Business;

public static class ModuleExtensions
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddSingleton<ITargetCurrencyCodeService, TargetCurrencyCodeService>();
        services.AddHttpClient();
        
        services.AddTransient<ICurrencyRateService, CurrencyRateService>();
        // Rate providers 
        services.AddTransient<IRateProvider, EcbExchangeRateProvider>();
        services.AddTransient<IRateProvider, DatabaseExchangeRateProvider>();
        services.AddTransient<IRateProvider, ReverseDatabaseExchangeRateProvider>();
        
        services.AddTransient<IImportBulkService, EcbImportBulkService>();
        
        services.AddTransient<ICurrencyExchangeRateIngressService, CurrencyExchangeRateIngressService>();
        
        return services;
    }
}