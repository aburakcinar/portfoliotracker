using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.AssetEntity;
using PortfolioTracker.WebApp.Business.Requests.AssetEntity;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Controllers;


[Route(@"api/[controller]")]
[ApiController]
public class AssetController
{
    private readonly IStockRepository m_stockRepository;
    private readonly IMediator m_mediator;

    public AssetController(IStockRepository stockRepository, IMediator mediator)
    {
        m_stockRepository = stockRepository;
        m_mediator = mediator;
    }
    
    [HttpGet(@"countries")]
    public IEnumerable<string> ListCountries() => m_stockRepository.GetCountries();
    
    [HttpGet(@"currencies")]
    public IEnumerable<string> ListCurrencies() => m_stockRepository.GetCurrencies();

    // [HttpGet(@"stocks/search")]
    // public IEnumerable<StockItemModel> SearchStocks([FromQuery] StockSearchRequest request)
    // {
    //     return m_stockRepository.SearchStocks(request);
    // }

    [HttpGet(@"summary")]
    public async Task<IEnumerable<AssetSummaryModel>> Summary()
    {
        return await m_mediator.Send(new ListAssetsSummaryRequest());
    }

    [HttpPost(@"stocks")]
    public async Task<bool> CreateStock([FromBody] CreateAssetStockCommand command)
    {
        return await m_mediator.Send(command);
    }

    [HttpGet(@"search")]
    public async Task<IEnumerable<AssetModel>> SearchAssets([FromQuery] SearchAssetRequest request)
    {
        return await m_mediator.Send(request);
    }

    [HttpGet(@"get/{id}")]
    public async Task<AssetModel?> GetAssetById([FromRoute] Guid id)
    {
        return await m_mediator.Send(new GetAssetRequest { Id = id });
    }
}