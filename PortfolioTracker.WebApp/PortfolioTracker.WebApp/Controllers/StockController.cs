using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Requests;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class StockController : Controller
{
    private readonly IMediator m_mediator;

    public StockController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<StockModel>> List()
    {
        return await m_mediator.Send(new ListStockRequest());
    }
}