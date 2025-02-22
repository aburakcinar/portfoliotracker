using System.Text.Json;

namespace PortfolioTracker.WebApp.Services;

public interface ILocaleService
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

public sealed class LocaleService : ILocaleService
{
    private readonly IWebHostEnvironment m_webHostEnvironment;

    public LocaleService(
        IWebHostEnvironment webHostEnvironment
        )
    {
        m_webHostEnvironment = webHostEnvironment;
    }

    public async Task<IEnumerable<LocaleItem>?> ListAsync()
    {
        var filePath = Path.Combine(m_webHostEnvironment.ContentRootPath, "Resources", @"locales.database.json");
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
        
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        
        return await JsonSerializer.DeserializeAsync<List<LocaleItem>>(stream, options);
    }
}