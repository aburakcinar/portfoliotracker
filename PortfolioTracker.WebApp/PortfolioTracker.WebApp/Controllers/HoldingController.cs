using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands;

namespace PortfolioTracker.WebApp.Controllers;

public sealed class HoldingReportSellRequest
{
    public decimal Price { get; init; }
    
    public decimal Quantity { get; init; }
    
    public decimal Expenses { get; init; }
    
    public DateTime ExecuteDate { get; init; }
}

[Route("api/[controller]")]
[ApiController]
public class HoldingController : Controller
{
    private readonly IMediator m_mediator;
    
    public HoldingController(IMediator mediator)
    {
        m_mediator = mediator;
    }
    
    
    [HttpPut(@"update")]
    public async Task<IActionResult> UpdateHolding( UpdateHoldingCommand request)
    {
        var result = await m_mediator.Send(request);
        
        return Ok(result);
    }

    [HttpDelete(@"delete/{holdingId}")]
    public async Task<bool> DeleteHolding([FromRoute] Guid holdingId)
    {
        return await m_mediator.Send(new DeleteHoldingCommand
        {
            HoldingId = holdingId
        });
    }

    [HttpPut(@"reportsell/{holdingId}")]
    public async Task<bool> ReportSell([FromRoute] Guid holdingId, HoldingReportSellRequest request)
    {
        var command = new ReportSellCommand
        {
            HoldingId = holdingId,
            Price = request.Price,
            Quantity = request.Quantity,
            Expenses = request.Expenses,
            ExecuteDate = request.ExecuteDate
        };
        
        return await m_mediator.Send(command);
    }
}