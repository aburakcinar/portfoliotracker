using System.Collections.Concurrent;
using System.Text.Json;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Services;

public interface ILocaleImporter
{
    Task ScanAsync();
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

public sealed class LocaleImporter : ILocaleImporter
{
    private readonly IWebHostEnvironment m_webHostEnvironment;
    private readonly PortfolioContext m_context;

    public LocaleImporter(
        IWebHostEnvironment webHostEnvironment,
        PortfolioContext context
        )
    {
        m_webHostEnvironment = webHostEnvironment;
        m_context = context;
    }

    public async Task ScanAsync()
    {
        var filePath = Path.Combine(m_webHostEnvironment.ContentRootPath, "Resources", @"locales.database.json");
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
        
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        
        var results = await JsonSerializer.DeserializeAsync<List<LocaleItem>>(stream, options);

        if (results == null)
        {
            return;
        }
        
        var projected = results.Select(x => new Locale
        {
            LocaleCode = x.Locale,
            LanguageName = x.Language.Name,
            LanguageNameLocal = x.Language.NameLocal,
            CountryName = x.Country.Name,
            CountryNameLocal = x.Country.NameLocal,
            CountryCode = x.Country.Code,
            CurrencyName = x.Country.Currency,
            CurrencyNameLocal = x.Country.CurrencyLocal,
            CurrencyCode = x.Country.CurrencyCode,
            CurrencySymbol = x.Country.CurrencySymbol,
            CurrencySubunitValue = x.Country.CurrencySubunitValue,
            CurrencySubunitName = x.Country.CurrencySubunitName,
        });

        m_context.RemoveRange(m_context.Locales.ToList());
        m_context.Locales.AddRange(projected);
        
        var result = await m_context.SaveChangesAsync();
    }
}