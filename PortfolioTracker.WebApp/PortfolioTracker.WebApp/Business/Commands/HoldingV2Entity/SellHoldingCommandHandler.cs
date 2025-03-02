using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.HoldingV2Entity;

public sealed class SellHoldingCommand : IRequest<bool>
{
    public Guid PortfolioId { get; set; }
    public Guid AssetId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Expenses { get; set; }
    public decimal Tax { get; set; }
    public DateTime ExecuteDate { get; set; }
}

public sealed class SellHoldingCommandHandler : IRequestHandler<SellHoldingCommand, bool>
{
    private readonly PortfolioContext m_context;

    public SellHoldingCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(SellHoldingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var executeDate = DateOnly.FromDateTime(request.ExecuteDate.ToLocalTime());

            if (request.Quantity <= 0)
            {
                return false;
            }

            var portfolio = await m_context.Portfolios
                .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

            var asset = await m_context.Assets
                .FirstOrDefaultAsync(x => x.Id == request.AssetId, cancellationToken);

            if (portfolio == null || asset == null)
            {
                return false;
            }

            // Calculate total holdings for validation
            var holdings = await m_context.Holdings
                .Include(h => h.BankAccountTransactionGroup)
                .ThenInclude(g => g.Transactions)
                .Where(h => h.PortfolioId == request.PortfolioId && h.AssetId == request.AssetId)
                .ToListAsync(cancellationToken);

            var totalQuantity = holdings.Sum(h => h.BankAccountTransactionGroup.Transactions
                .Where(t => t.ActionTypeCode == TransactionActionTypes.BUY_ASSET)
                .Sum(t => t.Quantity)) -
                holdings.Sum(h => h.BankAccountTransactionGroup.Transactions
                    .Where(t => t.ActionTypeCode == TransactionActionTypes.SELL_ASSET)
                    .Sum(t => t.Quantity));

            if (totalQuantity < request.Quantity)
            {
                return false; // Cannot sell more than owned
            }

            var transactionGroup = new BankAccountTransactionGroup
            {
                Id = Guid.NewGuid(),
                BankAccountId = portfolio.BankAccountId,
                Description = $"Sell {asset.Name} {request.Quantity} share",
                ExecuteDate = executeDate,
                Created = DateTime.Now.ToUniversalTime()
            };

            // Add main sell transaction
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now.ToUniversalTime(),
                Quantity = request.Quantity,
                InOut = InOut.Incoming,
                Price = request.Price,
                ActionTypeCode = TransactionActionTypes.SELL_ASSET,
                ActionType = m_context.TransactionActionTypes.First(x => x.Code == TransactionActionTypes.SELL_ASSET)
            });

            // Add expenses if any
            if (request.Expenses > 0)
            {
                transactionGroup.Transactions.Add(new BankAccountTransaction
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now.ToUniversalTime(),
                    Quantity = 1M,
                    InOut = InOut.Outgoing,
                    Price = request.Expenses,
                    ActionTypeCode = TransactionActionTypes.SELL_ASSET_FEE,
                    ActionType = m_context.TransactionActionTypes.First(x => x.Code == TransactionActionTypes.SELL_ASSET_FEE)
                });
            }

            // Add tax if any
            if (request.Tax > 0)
            {
                transactionGroup.Transactions.Add(new BankAccountTransaction
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now.ToUniversalTime(),
                    Quantity = 1M,
                    InOut = InOut.Outgoing,
                    Price = request.Tax,
                    ActionTypeCode = TransactionActionTypes.SELL_ASSET_TAX,
                    ActionType = m_context.TransactionActionTypes.First(x => x.Code == TransactionActionTypes.SELL_ASSET_TAX)
                });
            }

            m_context.Holdings.Add(new Holding
            {
                Id = Guid.NewGuid(),
                PortfolioId = request.PortfolioId,
                Portfolio = portfolio,
                AssetId = asset.Id,
                Asset = asset,
                BankAccountTransactionGroupId = transactionGroup.Id,
                BankAccountTransactionGroup = transactionGroup,
                ExecuteDate = executeDate,
                Created = DateTime.Now.ToUniversalTime(),
            });

            var count = await m_context.SaveChangesAsync(cancellationToken);
            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}