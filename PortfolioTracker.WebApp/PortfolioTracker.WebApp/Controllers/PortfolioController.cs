using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.DataStore;
using MediatR;
using PortfolioTracker.WebApp.Business.Commands;
using PortfolioTracker.WebApp.Business.Requests;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class PortfolioController : Controller
{
    private readonly PortfolioContext m_context;
    private readonly IMediator m_mediator;

    public PortfolioController(
        PortfolioContext mContext,
        IMediator mediator
        )
    {
        m_context = mContext;
        m_mediator = mediator;
    }

    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(nameof(PortfolioController));
    }

    public record CreatePortfolioRequestModel(string PortfolioName, string CurrencyCode);

    [HttpPost(@"create")]
    public async Task<IActionResult> CreatePortfolio([FromBody] CreatePortfolioRequestModel request)
    {
        var portfolioId = await m_mediator.Send(new CreatePortfolioCommand
        {
            Name = request.PortfolioName,
            CurrencyCode = request.CurrencyCode
        });
        
        return Ok(portfolioId);
    }

    [HttpGet(@"getinfo/{portfolioId}")]
    public async Task<IActionResult> GetPortfolioInfo([FromRoute]Guid portfolioId)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet(@"currencies")]
    public async Task<IEnumerable<CurrencyInfoModel>> ListCurrencies()
    {
        return await m_mediator.Send(new ListCurrenciesRequest());
    }

    [HttpGet(@"list")]
    public async Task<IEnumerable<PortfolioModel>> List()
    {
        return await m_mediator.Send(new ListPortfoliosRequest());
    }

    [HttpPost(@"buy")]
    public async Task<IActionResult> Buy([FromBody] BuyStockCommand request)
    {
        var result = await m_mediator.Send(request);

        return Ok(result);
    }
}