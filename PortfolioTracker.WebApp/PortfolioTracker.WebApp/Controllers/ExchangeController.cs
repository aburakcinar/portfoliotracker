using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.Business.Requests.Exchanges;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExchangeController : ControllerBase
{
    private readonly IExchangeService m_service;
    private readonly IMediator m_mediator;

    public ExchangeController(IExchangeService service, IMediator mediator)
    {
        m_service = service;
        m_mediator = mediator;
    }

    [HttpGet(@"query")]
    public async Task<IEnumerable<ExchangeQueryModel>> Query([FromQuery] string searchText,[FromQuery] int limit = 50)
    {
        return await m_mediator.Send(new SearchExhangeRequest { SearchText = searchText, Limit = limit });
    }

    [HttpGet(@"get/{mic}")]
    public async Task<ExchangeQueryModel?> Get([FromRoute]string mic)
    {
        return await m_mediator.Send(new GetExchangeRequest { Mic = mic });
    }
}