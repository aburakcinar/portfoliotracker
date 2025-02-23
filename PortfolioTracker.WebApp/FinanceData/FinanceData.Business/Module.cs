using FinanceData.Business.Api;
using FinanceData.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceData.Business;

public static class ModuleExtensions
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddSingleton<ITargetCurrencyCodeService, TargetCurrencyCodeService>();
        services.AddHttpClient();
        services.AddTransient<ICurrencyRateService, CurrencyRateService>();
        
        return services;
    }
}