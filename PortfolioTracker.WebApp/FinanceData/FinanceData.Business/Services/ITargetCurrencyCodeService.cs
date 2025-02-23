using System.Reflection;

namespace FinanceData.Business.Services;

public interface ITargetCurrencyCodeService
{
    IEnumerable<string> List();
    
    bool IsValid(string targetCurrencyCode);
}

public sealed class TargetCurrencies
{
    public const string USD = "USD";
    public const string EUR = "EUR";
    public const string TRY = "TRY";
}

internal sealed class TargetCurrencyCodeService : ITargetCurrencyCodeService
{
    private readonly Lazy<List<string>> m_currencyCodes = new Lazy<List<string>>(ScanCurrencyCodes);

    private static List<string> ScanCurrencyCodes()
    {
        var type = typeof(TargetCurrencies);

        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(x => x.Name)
            .ToList();
    }
    
    public IEnumerable<string> List()
    {
        return m_currencyCodes.Value;
    }

    public bool IsValid(string targetCurrencyCode)
    {
        return m_currencyCodes.Value.Contains(targetCurrencyCode);
    }
}