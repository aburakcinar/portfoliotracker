using MediatR;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class BuyStockCommand : IRequest<bool>
{
   public Guid PortfolioId { get; init; }
   
   public required string StockSymbol { get; init; } 
   
   public decimal Quantity { get; init; }
   
   public decimal Price { get; init; }

   public decimal Expenses { get; init; }
   
   public DateTime ExecuteDate { get; init; }
}


public class BuyStockCommandHandler : IRequestHandler<BuyStockCommand, bool>
{
    private readonly PortfolioContext m_context;

    public BuyStockCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(BuyStockCommand request, CancellationToken cancellationToken)
    {
        var stock = m_context.Stocks.FirstOrDefault(x => x.Symbol == request.StockSymbol);

        if (stock == null)
        {
            return false;
        }
        
        var portfolio = m_context.Portfolios.FirstOrDefault(x => x.Id == request.PortfolioId);

        if (portfolio == null)
        {
            return false;
        }

        try
        {
            portfolio.Holdings.Add(new Holding
            {
                Id = Guid.NewGuid(),
                Stock = stock,
                StockId = stock.Id,
                TransactionGroup = new TransactionGroup
                {
                    Id = Guid.NewGuid(),
                    Transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            Created = request.ExecuteDate,
                            Description = string.Empty,
                            Price = request.Price,
                            Quantity = request.Quantity,
                            Type = TransactionType.Investment,
                            InOut = InOut.In
                        },
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            Created = request.ExecuteDate,
                            Description = string.Empty,
                            Price = request.Expenses,
                            Quantity = 1.00M,
                            Type = TransactionType.InterestCommission,
                            InOut = InOut.In
                        },
                    }
                }
            });
        
            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch 
        {
            return false;
        }
    }
}