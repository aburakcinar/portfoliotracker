using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Requests;

namespace PortfolioTracker.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator m_mediator;

    public CurrencyController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<CurrencyInfoModel>> ListAsync()
    {
        return await m_mediator.Send(new ListCurrenciesRequest());
    }
}