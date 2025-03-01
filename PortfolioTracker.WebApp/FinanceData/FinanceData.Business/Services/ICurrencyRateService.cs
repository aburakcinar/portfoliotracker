using FinanceData.Business.Api;
using FinanceData.Business.DataStore;
using FinanceData.Business.Utils;
using Microsoft.EntityFrameworkCore;

namespace FinanceData.Business.Services;

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
    private readonly IFinansDataContext m_context;

    //private readonly BaseRateProvider m_ecbRateProvider ;
    private readonly BaseRateProvider? m_rateProvider;

    public CurrencyRateService(
        ITargetCurrencyCodeService targetCurrencyCodeService,
        IEnumerable<IRateProvider> rateProviders,
        IFinansDataContext context
    )
    {
        m_targetCurrencyCodeService = targetCurrencyCodeService;
        m_context = context;
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

        return rate * amount;
    }

    public async Task<GetCurrencyRatesTimeseriesResult> GetTimeSeriesAsync(GetCurrencyRatesTimeseriesQuery query)
    {
        if (query.EndDate.Subtract(query.StartDate).Days > 365)
        {
            return new GetCurrencyRatesTimeseriesResult
            {
                Success = false,
            };
        }

        var startDate = query.StartDate.ToUniversalTime();
        var endDate = query.EndDate.ToUniversalTime();

        var items = await m_context
            .CurrencyExchangeRatios
            .Where(x =>
                x.Date >= startDate &&
                x.Date <= endDate &&
                x.BaseCurrency == query.BaseCurrency &&
                x.TargetCurrency == query.TargetCurrency
            ).ToListAsync();

        if (items.Count > 0)
        {
            var rates = items
                .Select(x => new CurrencyExchangeRateItem(x.Date.Date, x.Rate))
                .ToList();

            return new GetCurrencyRatesTimeseriesResult
            {
                Success = true,
                BaseCurrency = query.BaseCurrency,
                TargetCurrency = query.TargetCurrency,
                Rates = rates
            };
        }

        items = await m_context
            .CurrencyExchangeRatios
            .Where(x =>
                x.Date >= startDate &&
                x.Date <= endDate &&
                x.BaseCurrency == query.TargetCurrency &&
                x.TargetCurrency == query.BaseCurrency
            ).ToListAsync();

        if (items.Count > 0)
        {
            var rates = items
                .Select(x => new CurrencyExchangeRateItem(x.Date.Date, 1 / x.Rate))
                .ToList();

            return new GetCurrencyRatesTimeseriesResult
            {
                Success = true,
                BaseCurrency = query.BaseCurrency,
                TargetCurrency = query.TargetCurrency,
                Rates = rates
            };
        }
        
        return new GetCurrencyRatesTimeseriesResult
        {
            Success = false,
        };
    }
}