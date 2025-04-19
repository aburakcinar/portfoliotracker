using System.Reflection;
using System.Text.Json;
using PortfolioTracker.Data.Migrations;

namespace PortfolioTracker.WebApp.Services;

public interface ILocalesReader
{
    Task<IEnumerable<LocaleItem>?> ListAsync();
}

public class LocaleItem
{
    public required string Locale { get; set; }

    public required LanguageItem Language { get; set; }
    
    public required CountryItem Country { get; set; }
}

public class LanguageItem
{
    public required string Name { get; set; }
    
    public required string NameLocal { get; set; }
}

public class CountryItem
{
    public required string Name { get; set; }
    
    public required string NameLocal { get; set; }
    
    public required string Code { get; set; }
    
    public required string Currency { get; set; }
    
    public required string CurrencyLocal { get; set; }
    
    public required string CurrencyCode { get; set; }
    
    public required string CurrencySymbol { get; set; }

    public int CurrencySubunitValue { get; set; }

    public required string CurrencySubunitName { get; set; }
}

public sealed class CvsLocalesReader : ILocalesReader
{
    public async Task<IEnumerable<LocaleItem>?> ListAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $@"{typeof(ApiDbInitializer).Namespace}.Resources.locales.database.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
        
        return await JsonSerializer.DeserializeAsync<List<LocaleItem>>(stream, options);
    }
}