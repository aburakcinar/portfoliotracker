using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.BankTransactionEntity;

namespace PortfolioTracker.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImportController : ControllerBase
{
    private readonly IMediator m_mediator;

    public ImportController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet(@"health")]
    public IActionResult Health()
    {
        return Ok(@"Healthy");
    }

    [HttpPost(@"portfolio/{portfolioId}/sourcetype/{importSourceType}")]
    public async Task<IActionResult> Import([FromRoute]Guid portfolioId, [FromRoute]string importSourceType, [FromForm] IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest(@"No file");
        }
        
        var stream = file.OpenReadStream();
        
        var result = await m_mediator.Send(new ImportTransactionsFromFileCommand
        {
            ImportSourceType = importSourceType,
            PortfolioId = portfolioId,
            DataStream = stream,
        });

        return Ok(result);

        //return await Task.FromResult(false);
    }
}