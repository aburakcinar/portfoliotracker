using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Requests;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly IMediator m_mediator;

    public TransactionController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet(@"list")]
    public async Task<IEnumerable<TransactionViewModel>> List()
    {
        return await m_mediator.Send(new ListTransactionsRequest());
    }
}