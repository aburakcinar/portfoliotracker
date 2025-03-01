using FinanceData.Business.Api;
using FinanceData.Business.DataStore;
using FinanceData.Business.Services;
using FinanceData.Business.Services.ExchangeRateProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinanceData.Business;

public static class ModuleExtensions
{
    public static IServiceCollection AddFinanceDataBusiness(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddSingleton<ITargetCurrencyCodeService, TargetCurrencyCodeService>();
        services.AddHttpClient();
        
        services.AddTransient<ICurrencyRateService, CurrencyRateService>();
        // Rate providers 
        services.AddTransient<IRateProvider, EcbExchangeRateProvider>();
        services.AddTransient<IRateProvider, DatabaseExchangeRateProvider>();
        services.AddTransient<IRateProvider, ReverseDatabaseExchangeRateProvider>();
        
        services.AddTransient<IImportBulkService, EcbImportBulkService>();
        
        services.AddTransient<ICurrencyExchangeRateIngressService, CurrencyExchangeRateIngressService>();
        
        var connectionString = builder.Configuration.GetConnectionString(@"TimescaleDb-FinanceData");
        builder.Services.AddDbContext<FinansDataContext>(options => options.UseNpgsql(connectionString));
        builder.Services.AddTransient<IFinansDataContext>(x => x.GetRequiredService<FinansDataContext>());

        
        return services;
    }
}