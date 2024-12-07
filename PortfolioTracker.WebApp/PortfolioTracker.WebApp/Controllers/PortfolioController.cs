using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Controllers;

public class BuyReqestModel
{
    public string Symbol { get; init; } = String.Empty;

    public decimal Price { get; init; }

    public int Quantity { get; init; }

    public decimal Expenses { get; init; }

    public DateTime ExecuteDate { get; init; }
}

public class PortfolioController : Controller
{
    private readonly PortfolioContext m_context;

    private const string BUY = @"BUY";
    private const string SELL = @"SELL";
    private const string EXPENSE = @"EXPENSE";

    public PortfolioController(PortfolioContext mContext)
    {
        m_context = mContext;
    }

    // GET
    public IActionResult Index()
    {
        return Ok(nameof(PortfolioController));
    }

    [HttpGet(@"list")]
    public IActionResult List()
    {
        // m_context.StockPurchases
        //     .GroupBy(x => x.Symbol)
        //     .
        
        return Ok();
    }

    [HttpPost(@"buy")]
    public async Task<IActionResult> Buy([FromBody] BuyReqestModel request)
    {
        var purchase = new StockPurchase
        {
            Id = Guid.NewGuid(),
            Symbol = request.Symbol,
            TransactionGroup = new TransactionGroup
            {
                Id = Guid.NewGuid(),
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Id = Guid.NewGuid(),
                        Price = request.Price,
                        Quantity = request.Quantity,
                        Created = request.ExecuteDate,
                        InOut = InOut.In,
                        Type = BUY,
                        Comment = @$"{request.Symbol} buy in."
                    },
                    new Transaction
                    {
                        Id = Guid.NewGuid(),
                        Price = request.Expenses,
                        Quantity = (decimal)1.0,
                        Created = request.ExecuteDate,
                        InOut = InOut.In,
                        Type = EXPENSE,
                        Comment = @$"{request.Symbol} buy in expenses."
                    }
                }
            }
        };

        m_context.StockPurchases.Add(purchase);

        await m_context.SaveChangesAsync();

        return Ok();
    }
}