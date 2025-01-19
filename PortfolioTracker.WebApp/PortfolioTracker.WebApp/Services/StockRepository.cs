using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace PortfolioTracker.WebApp.Services;

public class StockItemModel
{
    [Name(@"country")]
    public required string Country { get; init; }
    
    [Name(@"name")]
    public required string Name { get; init; }
    
    [Name(@"full_name")]
    public required string FullName { get; init; }
    
    [Name(@"tag")]
    public required string Tag { get; init; }
    
    [Name(@"isin")]
    public required string Isin { get; init; }
    
    [Name(@"id")]
    public int Id { get; init; }
    
    [Name(@"currency")]
    public required string Currency { get; init; }
    
    [Name(@"symbol")]
    public required string Symbol { get; init; }
}
//
// public sealed class StockSearchRequest
// {
//     public required string SearchText { get; init; }
//     
//     public string[]? Countries { get; init; }
//     
//     public int PageIndex { get; init; }
//     
//     public int PageSize { get; init; }
// }

public interface IStockRepository
{
    void Load();

    IEnumerable<string> GetCountries();

    IEnumerable<string> GetCurrencies();
    
    //IEnumerable<StockItemModel> SearchStocks(StockSearchRequest request);
}

internal sealed class StockRepository : IStockRepository
{
    private readonly IWebHostEnvironment m_webHostEnvironment;
    private List<StockItemModel> m_stocks = new();
    private List<string> m_countries = new();
    private List<string> m_currencies = new();

    public StockRepository(IWebHostEnvironment webHostEnvironment)
    {
        m_webHostEnvironment = webHostEnvironment;
    }
    
    public void Load()
    {
        var filePath = Path.Combine(m_webHostEnvironment.ContentRootPath, "Resources", @"stocks.csv");
        
        using var reader = new StreamReader(filePath);
        using var cvs = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        m_stocks = cvs.GetRecords<StockItemModel>().ToList();
        
        FillCountries();
        FillCurrencies();
    }

    private void FillCountries()
    {
        m_countries = m_stocks.Select(x => x.Country).Distinct().ToList();
    }

    private void FillCurrencies()
    {
        m_currencies = m_stocks.Select(x => x.Currency).Distinct().ToList();
    }

    public IEnumerable<string> GetCountries()
    {
        return m_countries.AsReadOnly();
    }
    
    public IEnumerable<string> GetCurrencies()
    {
        return m_currencies.AsReadOnly();
    }

    // public IEnumerable<StockItemModel> SearchStocks(StockSearchRequest request)
    // {
    //     if (string.IsNullOrWhiteSpace(request.SearchText))
    //     {
    //         return Enumerable.Empty<StockItemModel>();
    //     }
    //     
    //     var searchText = request.SearchText.ToLowerInvariant();
    //
    //     return m_stocks
    //         .Where(x => request.Countries == null ||
    //                     request.Countries.Length == 0 ||
    //                     request.Countries.Contains(x.Country))
    //         .Where(x => x.Symbol.ToLowerInvariant().Contains(searchText)
    //                     || x.FullName.ToLowerInvariant().Contains(searchText)
    //                     || x.Name.ToLowerInvariant().Contains(searchText))
    //         .Skip(request.PageIndex * request.PageSize)
    //         .Take(request.PageSize);
    // }
}

public static class StockRepositoryExtensions
{
    public static IApplicationBuilder UseStockRepository(this IApplicationBuilder builder)
    {
        var repository = builder.ApplicationServices.GetService<IStockRepository>();
        
        repository?.Load();
        
        return builder;
    }
}