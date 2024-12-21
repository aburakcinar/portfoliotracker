using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.DataStore;
using MediatR;
using PortfolioTracker.WebApp.Business.Commands;
using PortfolioTracker.WebApp.Business.Requests;

namespace PortfolioTracker.WebApp.Controllers;

public class BuyRequestModel
{
    public string Symbol { get; init; } = String.Empty;

    public decimal Price { get; init; }

    public int Quantity { get; init; }

    public decimal Expenses { get; init; }

    public DateTime ExecuteDate { get; init; }
}

[Route(@"api/[controller]")]
[ApiController]
public class PortfolioController : Controller
{
    private readonly PortfolioContext m_context;
    private readonly IMediator m_mediator;

    private const string BUY = @"BUY";
    private const string SELL = @"SELL";
    private const string EXPENSE = @"EXPENSE";

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
    public async Task<IActionResult> Buy([FromBody] BuyRequestModel request)
    {
        // var purchase = new StockPurchase
        // {
        //     Id = Guid.NewGuid(),
        //     Symbol = request.Symbol,
        //     TransactionGroup = new TransactionGroup
        //     {
        //         Id = Guid.NewGuid(),
        //         Transactions = new List<Transaction>
        //         {
        //             new Transaction
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Price = request.Price,
        //                 Quantity = request.Quantity,
        //                 Created = request.ExecuteDate,
        //                 InOut = InOut.In,
        //                 Type = BUY,
        //                 Comment = @$"{request.Symbol} buy in."
        //             },
        //             new Transaction
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Price = request.Expenses,
        //                 Quantity = (decimal)1.0,
        //                 Created = request.ExecuteDate,
        //                 InOut = InOut.In,
        //                 Type = EXPENSE,
        //                 Comment = @$"{request.Symbol} buy in expenses."
        //             }
        //         }
        //     }
        // };
        //
        // m_context.StockPurchases.Add(purchase);
        //
        // await m_context.SaveChangesAsync();

        await Task.CompletedTask;

        return Ok();
    }
}