using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class ReserveStockInPortfolioCommand : IRequest<bool>
{
    public Guid PortfolioId { get; init; }
    
    public required string StockSymbol { get; init; }
}

public sealed class ReserveStockInPortfolioCommandHandler : IRequestHandler<ReserveStockInPortfolioCommand, bool>
{
    private readonly PortfolioContext m_context;

    public ReserveStockInPortfolioCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(ReserveStockInPortfolioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stock = m_context.Stocks.FirstOrDefault(x => x.Symbol == request.StockSymbol);

            if (stock == null)
            {
                return false;
            }
            
            var portfolio = m_context
                .Portfolios
                .Include(x => x.Holdings)
                .ThenInclude(holding => holding.Stock)
                .FirstOrDefault(x => x.Id == request.PortfolioId);
            
            if (portfolio == null)
            {
                return false;
            }

            if (portfolio.Holdings.Any(x => x.StockId == stock.Id) )
            {
                // Stock is already existed in target portfolio
                return false;
            }
            
            m_context.Holdings.Add(new Holding
            {
                Id = Guid.NewGuid(),
                Portfolio = portfolio,
                Stock = stock,
                TransactionGroup = new TransactionGroup
                {
                    Id = Guid.NewGuid(),
                    Transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.MinValue,
                            Description = string.Empty,
                            Price = 0M,
                            Quantity = 0M,
                            Type = TransactionType.Investment,
                            InOut = InOut.In
                        },
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.MinValue,
                            Description = string.Empty,
                            Price = 0M,
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