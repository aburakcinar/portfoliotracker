using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands;
using PortfolioTracker.WebApp.Business.Commands.PortfolioV2Entity;
using PortfolioTracker.WebApp.Business.Requests;
using PortfolioTracker.WebApp.Business.Requests.PortfolioV2Entity;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class PortfolioController : Controller
{
    private readonly IMediator m_mediator;

    public PortfolioController(
        IMediator mediator
        )
    {
        m_mediator = mediator;
    }

    public record CreatePortfolioRequestModel(string Name, string Description);

    [HttpPost()]
    public async Task<IActionResult> CreatePortfolio([FromBody] CreatePortfolioRequestModel request)
    {
        var portfolioId = await m_mediator.Send(new CreatePortfolioV2Command
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        });
        
        return Ok(portfolioId);
    }

    [HttpGet()]
    public async Task<IEnumerable<PortfolioModel>> List()
    {
        return await m_mediator.Send(new ListPortfoliosRequest());
    }

    [HttpGet(@"{portfolioId}")]
    public async Task<PortfolioModel?> GetPortfolio([FromRoute]Guid portfolioId)
    {
        return await m_mediator.Send(new GetPortfolioRequest { PortfolioId = portfolioId });
    }
    
    
    
    //
    //
    // [HttpGet(@"getinfo/{portfolioId}")]
    // public async Task<IActionResult> GetPortfolioInfo([FromRoute]Guid portfolioId)
    // {
    //     await Task.CompletedTask;
    //     return Ok();
    // }
    //
    // [HttpGet(@"currencies")]
    // public async Task<IEnumerable<CurrencyInfoModel>> ListCurrencies()
    // {
    //     return await m_mediator.Send(new ListCurrenciesRequest());
    // }
    //
    //
    //
    // [HttpPost(@"buy")]
    // public async Task<IActionResult> Buy([FromBody] BuyStockCommand request)
    // {
    //     var result = await m_mediator.Send(request);
    //
    //     return Ok(result);
    // }
    //
    // [HttpPost(@"holdings/{portfolioId}/reserve/{stockSymbol}")]
    // public async Task<IActionResult> ReserveStockOnPortfolio([FromRoute] Guid portfolioId,[FromRoute] string stockSymbol)
    // {
    //     var result = await m_mediator.Send(new ReserveStockInPortfolioCommand
    //     {
    //         PortfolioId = portfolioId,
    //         StockSymbol = stockSymbol
    //     });
    //
    //     return Ok(result);
    // }
    //
    // [HttpGet(@"holdings/{portfolioId}")]
    // public async Task<IActionResult> GetHoldings([FromRoute] Guid portfolioId)
    // {
    //     var result = await m_mediator.Send(new GetPortfolioHoldingsRequest{ PortfolioId = portfolioId });
    //
    //     return Ok(result);
    // }
    //
    // [HttpGet(@"holdings/{portfolioId}/details/{stockSymbol}")]
    // public async Task<IActionResult> ListHoldingDetails([FromRoute] Guid portfolioId, [FromRoute] string stockSymbol)
    // {
    //     var result = await m_mediator.Send(new ListHoldingDetailsRequest
    //     {
    //         PortfolioId = portfolioId,
    //         StockSymbol = stockSymbol
    //     });
    //     
    //     return Ok(result);
    // }
}