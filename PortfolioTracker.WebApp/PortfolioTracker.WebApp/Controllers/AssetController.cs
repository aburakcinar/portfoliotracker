using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Controllers;


[Route(@"api/[controller]")]
[ApiController]
public class AssetController
{
    private readonly IStockRepository m_stockRepository;

    public AssetController(IStockRepository stockRepository)
    {
        m_stockRepository = stockRepository;
    }
    
    [HttpGet(@"countries")]
    public IEnumerable<string> ListCountries() => m_stockRepository.GetCountries();
    
    [HttpGet(@"currencies")]
    public IEnumerable<string> ListCurrencies() => m_stockRepository.GetCurrencies();

    [HttpGet(@"stocks/search")]
    public IEnumerable<StockItemModel> SearchStocks([FromQuery] StockSearchRequest request)
    {
        return m_stockRepository.SearchStocks(request);
    }
}