using FinanceData.Business.DataStore;

namespace FinanceData.Business.Services;

public interface ICurrencyExchangeRateIngressService
{
    Task<int> IngressAsync(IEnumerable<CurrencyExchangeRatio> items);
}

internal sealed class CurrencyExchangeRateIngressService : ICurrencyExchangeRateIngressService
{
    private readonly IFinansDataContext m_context;

    public CurrencyExchangeRateIngressService(IFinansDataContext context)
    {
        m_context = context;
    }

    public async Task<int> IngressAsync(IEnumerable<CurrencyExchangeRatio> items)
    {
        var groupedItems = items.GroupBy(x =>
            new
            {
                BaseCurrency = x.BaseCurrency,
                TargetCurrency = x.TargetCurrency
            });
        
        var itemsToInsert = new List<CurrencyExchangeRatio>();

        foreach (var group in groupedItems)
        {
            var baseCurrency = group.Key.BaseCurrency;
            var targetCurrency = group.Key.TargetCurrency;
            var startDate = group.Min(x => x.Date);
            var endDate = group.Max(x => x.Date);

            var existing = m_context
                .CurrencyExchangeRatios
                .Where(x =>
                    x.BaseCurrency == baseCurrency &&
                    x.TargetCurrency == targetCurrency &&
                    x.Date >= startDate &&
                    x.Date <= endDate)
                .ToList()
                .Select(x => x.AsKey());
            
            itemsToInsert.AddRange(group.Where(x => !existing.Contains(x.AsKey())));
        }

        await m_context.CurrencyExchangeRatios.AddRangeAsync(itemsToInsert);
        return await m_context.SaveChangesAsync();
    }
}