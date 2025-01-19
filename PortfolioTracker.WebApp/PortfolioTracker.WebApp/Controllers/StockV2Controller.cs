using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.Stocks;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.Business.Requests.Stocks;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/stock/v2")]
[ApiController]
public class StockV2Controller : ControllerBase
{
    private readonly IMediator m_mediator;

    public StockV2Controller(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpPost]
    public async Task<bool> Create([FromBody] CreateStockCommand command)
    {
        return await m_mediator.Send(command);
    }

    [HttpPut]
    public async Task<bool> Update([FromBody] UpdateStockCommand command)
    {
        return await m_mediator.Send(command);
    }
    
    [HttpDelete(@"{fullStockCode}")]
    public async Task<bool> Delete([FromRoute]string fullStockCode)
    {
        return await m_mediator.Send(new DeleteStockCommand() { FullCode = fullStockCode });
    }

    [HttpGet(@"{fullStockCode}")]
    public async Task<StockItemModel?> Get([FromRoute]string fullStockCode)
    {
        return await m_mediator.Send(new GetStockRequest { FullCode = fullStockCode });
    }

    [HttpGet(@"list")]
    public async Task<IEnumerable<StockItemModel>> GetAll()
    {
        return await m_mediator.Send(new ListStocksRequest());
    }
}