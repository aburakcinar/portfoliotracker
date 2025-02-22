using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.PortfolioV2Entity;
using PortfolioTracker.WebApp.Business.Requests.PortfolioV2Entity;

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

    [HttpPost()]
    public async Task<IActionResult> CreatePortfolio([FromBody] CreatePortfolioV2Command command)
    {
        var result = await m_mediator.Send(command);
        
        return Ok(result);
    }
    
    [HttpPut()]
    public async Task<IActionResult> UpdatePortfolio([FromBody] UpdatePortfolioV2Command command)
    {
        var result = await m_mediator.Send(command);
        
        return Ok(result);
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

    [HttpGet(@"total/{portfolioId}")]
    public async Task<PortfolioTotalPositionResultModel?> GetPortfolioTotalPosition([FromRoute] Guid portfolioId)
    {
        return await m_mediator.Send(new GetPortfolioTotalPositionRequest { PortfolioId = portfolioId });
    }
}