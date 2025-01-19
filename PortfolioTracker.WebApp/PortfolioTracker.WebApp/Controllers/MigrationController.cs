using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.Migrations;

namespace PortfolioTracker.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MigrationController : ControllerBase
{
    private readonly IMediator m_mediator;

    public MigrationController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet(@"health")]
    public IActionResult HealthCheck()
    {
        return Ok(this.GetType().Name);
    }

    [HttpPost("currencyfromlocales")]
    public async Task<bool> MigrateCurrenciesFromLocalesAsync()
    {
        return await m_mediator.Send(new MigrateCurrenciesFromLocalesCommand());
    }
}