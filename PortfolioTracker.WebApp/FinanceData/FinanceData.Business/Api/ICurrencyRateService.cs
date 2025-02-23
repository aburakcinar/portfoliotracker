namespace FinanceData.Business.Api;

public sealed class CurrencyRateQueryModel
{
    public required string Base { get; init; }
    
    public required string Target { get; init; }
    
    public DateOnly Date { get; init; }
}

public sealed class CurrencyRateResult
{
    public required string BaseCurrency { get; init; }

    public required string TargetCurrency { get; init; }
    
    public decimal Rate { get; init; }
    
    public DateOnly Date { get; init; }
}

public interface ICurrencyRateService
{
    Task<decimal> GetRateAsync(CurrencyRateQueryModel query);
    
    Task<decimal> ConvertAsync(decimal amount, CurrencyRateQueryModel query);
}
