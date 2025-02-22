using System.Text.Json;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Services;

public sealed class AssetItem
{
    public Guid Id { get; set; }

    public string TickerSymbol { get; set; } = string.Empty;

    public string ExchangeCode { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public AssetTypes AssetType { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Isin { get; set; } = string.Empty;

    public string Wkn { get; set; } = string.Empty;

    public string WebSite { get; set; } = string.Empty;
} 

public interface IAssetService
{
    Task<IEnumerable<AssetItem>?> ListAsync();
}

public sealed class AssetService : IAssetService
{
    private readonly IWebHostEnvironment m_webHostEnvironment;

    public AssetService(IWebHostEnvironment webHostEnvironment)
    {
        m_webHostEnvironment = webHostEnvironment;
    }

    public async Task<IEnumerable<AssetItem>?> ListAsync()
    {
        var filePath = Path.Combine(m_webHostEnvironment.ContentRootPath, "Resources", @"assets.database.json");
        
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var result = await JsonSerializer.DeserializeAsync<List<AssetItem>>(stream,options);

        return result;
    }
}