using FinanceData.Business.Api;
using FinanceData.Business.DataStore;

namespace FinanceData.Business.Services;

public static class Undefined
{
    public static decimal Decimal => -999.25M;

    public static double Double => -999.25D;

    public static bool IsUndefined(decimal value)
    {
        return decimal.Equals(value, Undefined.Decimal);
    }
}

public interface IRateProvider
{
    int Order { get; }
    
    Task<decimal> GetRateAsync(CurrencyRateQueryModel query);
}

internal abstract class BaseRateProvider : IRateProvider
{
    private BaseRateProvider? m_parent;
    
    public abstract int Order { get; }
    
    public abstract Task<decimal> GetRateAsync(CurrencyRateQueryModel query);

    internal void SetParent(BaseRateProvider parent)
    {
        m_parent = parent;
    }

    protected async Task<decimal> ExecuteParentAsync(CurrencyRateQueryModel query)
    {
        if (m_parent == null)
        {
            return Undefined.Decimal;
        }

        return await m_parent.GetRateAsync(query);
    }
} 

internal sealed class CurrencyRateService : ICurrencyRateService
{
    private readonly ITargetCurrencyCodeService m_targetCurrencyCodeService;
    
    //private readonly BaseRateProvider m_ecbRateProvider ;
    private readonly BaseRateProvider? m_rateProvider;

    public CurrencyRateService(
        ITargetCurrencyCodeService targetCurrencyCodeService,
        IEnumerable<IRateProvider> rateProviders
        )
    {
        m_targetCurrencyCodeService = targetCurrencyCodeService;
        //m_ecbRateProvider = new EcbExchangeRateProvider(httpClientFactory, context);

        var lst = rateProviders
            .OfType<BaseRateProvider>()
            .OrderByDescending(x => x.Order);

        foreach (var provider in lst)
        {
            if (m_rateProvider is not null)
            {
                provider.SetParent(m_rateProvider);
            }
            
            m_rateProvider = provider;
        }
    }
    
    public async Task<decimal> GetRateAsync(CurrencyRateQueryModel query)
    {
        if (query.Base.Equals(query.Target))
        {
            return 1M;
        }
        
        if (!m_targetCurrencyCodeService.IsValid(query.Base) || !m_targetCurrencyCodeService.IsValid(query.Target))
        {
            return Undefined.Decimal;
        }

        return await m_rateProvider!.GetRateAsync(query);
    }

    public async Task<decimal> ConvertAsync(decimal amount, CurrencyRateQueryModel query)
    {
        var rate = await GetRateAsync(query);

        if (Undefined.IsUndefined(rate))
        {
            return Undefined.Decimal;
        }
        
        return  rate * amount;
    }
} 