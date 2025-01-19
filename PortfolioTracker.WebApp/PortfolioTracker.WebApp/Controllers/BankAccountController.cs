using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.BankAccountEntity;

namespace PortfolioTracker.WebApp.Controllers;

[ApiController]
[Route(@"api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator m_mediator;

    public BankAccountController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet(@"health")]
    public IActionResult Health()
    {
        return Ok(this.GetType().Name);
    }

    [HttpPost]
    public async Task<bool> Create(CreateBankAccountCommand command)
    {
        return await m_mediator.Send(command);
    }
}