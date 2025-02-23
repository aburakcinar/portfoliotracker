using FinanceData.Business.Api;

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

internal abstract class BaseRateProvider
{
    private BaseRateProvider? m_parent;
    public abstract Task<decimal> GetRateAsync(CurrencyRateQueryModel query);

    internal void SetParent(BaseRateProvider parent)
    {
        m_parent = parent;
    }
} 

internal sealed class CurrencyRateService : ICurrencyRateService
{
    private readonly ITargetCurrencyCodeService m_targetCurrencyCodeService;
    
    private readonly BaseRateProvider m_ecbRateProvider ;

    public CurrencyRateService(
        ITargetCurrencyCodeService targetCurrencyCodeService,
        IHttpClientFactory httpClientFactory
        )
    {
        m_targetCurrencyCodeService = targetCurrencyCodeService;
        m_ecbRateProvider = new EcbExchangeRateProvider(httpClientFactory);
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

        return await m_ecbRateProvider.GetRateAsync(query);
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