using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.HoldingV2Entity;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.Business.Requests.Holdings;
using PortfolioTracker.WebApp.Business.Requests.HoldingV2Entity;

namespace PortfolioTracker.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HoldingController : Controller
{
    private readonly IMediator m_mediator;

    public HoldingController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet(@"listbyportfolio/{portfolioId}")]
    public async Task<IEnumerable<HoldingAggregateModel>> ListByPortfolioId([FromRoute] Guid portfolioId)
    {
        return await m_mediator.Send(new ListHoldingsByPortfolioRequest { PortfolioId = portfolioId });
    }

    [HttpGet(@"listbyportfolio/{portfolioId}/asset/{assetId}")]
    public async Task<IEnumerable<HoldingDetailModel>> ListByAssetAsync(
        [FromRoute] Guid portfolioId,
        [FromRoute] Guid assetId
    )
    {
        return await m_mediator.Send(new ListHoldingsByAssetRequest
        {
            PortfolioId = portfolioId,
            AssetId = assetId
        });
    }

    [HttpGet(@"total/portfolio/{portfolioId}/asset/{assetId}")]
    public async Task<HoldingTotalPositionResultModel?> GetHoldingTotalPosition([FromRoute] Guid portfolioId,
        [FromRoute] Guid assetId)
    {
        return await m_mediator.Send(new GetHoldingTotalPositionRequest { PortfolioId = portfolioId, AssetId = assetId });
    }

    [HttpGet(@"transactions/portfolio/{portfolioId}/asset/{assetId}")]
    public async Task<IEnumerable<HoldingAssetTransactionModel>> ListAssetTransactions([FromRoute] Guid portfolioId,
        [FromRoute] Guid assetId)
    {
        return await m_mediator.Send(new ListHoldingAssetTransactionsRequest
        {
            PortfolioId = portfolioId,
            AssetId = assetId
        });
    }

    //[HttpGet(@"asset/summary/portfolio/{portfolioId}/asset/{assetId}")]
    //public async Task<HoldingAssetSummaryModel> GetHoldingAssetSummary([FromRoute] Guid portfolioId, [FromRoute] Guid assetId)
    //{
    //    return await m_mediator.Send(new GetHoldingAssetSummaryRequest
    //    {
    //        PortfolioId = portfolioId,
    //        AssetId = assetId
    //    });
    //}

    [HttpPost(@"reportbuy")]
    public async Task<bool> AddHolding([FromBody] AddHoldingCommand command)
    {
        return await m_mediator.Send(command);
    }

    [HttpPost("reportsell")]
    public async Task<bool> SellHolding([FromBody] SellHoldingCommand request)
    {
        return await m_mediator.Send(request);
    }
}