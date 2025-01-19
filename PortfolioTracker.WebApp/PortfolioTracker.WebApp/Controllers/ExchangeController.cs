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
    private readonly IExchangeImporter m_importer;
    private readonly IMediator m_mediator;

    public ExchangeController(IExchangeImporter importer, IMediator mediator)
    {
        m_importer = importer;
        m_mediator = mediator;
    }

    [HttpPost]
    public IActionResult Scan()
    {
        var result = m_importer.Scan();

        return Ok(result);
    }

    [HttpGet(@"query")]
    public async Task<IEnumerable<ExchangeQueryModel>> Query([FromQuery] string searchText,[FromQuery] int limit = 50)
    {
        return await m_mediator.Send(new SearchExhangeRequest { SearchText = searchText, Limit = limit });
    }
}