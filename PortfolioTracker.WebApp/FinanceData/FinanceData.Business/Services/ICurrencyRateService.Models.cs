namespace FinanceData.Business.Services;

public record GetCurrencyRatesTimeseriesQuery(DateTime StartDate, DateTime EndDate, string BaseCurrency, string TargetCurrency);

public abstract class BaseResult
{
    public bool Success { get; init; }
    
    public abstract string RequestName { get;  }
}

public record CurrencyExchangeRateItem(DateTime Date, decimal Rate);

public sealed class GetCurrencyRatesTimeseriesResult : BaseResult
{
    public override string RequestName => nameof(GetCurrencyRatesTimeseriesQuery);
    
    public string BaseCurrency { get; init; } = string.Empty; 
    
    public string TargetCurrency { get; init; } = string.Empty;

    public List<CurrencyExchangeRateItem> Rates { get; init; } = new();
}