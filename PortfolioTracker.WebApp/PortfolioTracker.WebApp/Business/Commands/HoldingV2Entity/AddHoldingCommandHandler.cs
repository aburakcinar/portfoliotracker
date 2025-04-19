using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Commands.HoldingV2Entity;

public sealed class AddHoldingCommand : IRequest<bool>
{
    public Guid PortfolioId { get; set; }

    public Guid AssetId { get; set; }

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Expenses { get; set; }

    public DateTime ExecuteDate { get; set; }
}

public sealed class AddHoldingCommandHandler : IRequestHandler<AddHoldingCommand, bool>
{
    private readonly IPortfolioContext m_context;

    public AddHoldingCommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(AddHoldingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var executeDate = DateOnly.FromDateTime(request.ExecuteDate.ToLocalTime());
            
            if (request.Quantity == 0)
            {
                return false;
            }
            
            var portfolio = await m_context
                .Portfolios
                .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken: cancellationToken);

            var asset = await m_context.Assets.FirstOrDefaultAsync(x => x.Id == request.AssetId,
                cancellationToken: cancellationToken);

            if (portfolio == null || asset == null)
            {
                return false;
            }

            var transactionGroup = new BankAccountTransactionGroup()
            {
                Id = Guid.NewGuid(),
                BankAccountId = portfolio.BankAccountId,
                Description = $@"Buy {asset.Name} {request.Quantity} share",
                ExecuteDate = executeDate,
                Created = DateTime.Now.ToUniversalTime()
            };

            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now.ToUniversalTime(),
                Quantity = request.Quantity,
                InOut = InOut.Outgoing,
                Price = request.Price,
                ActionTypeCode = TransactionActionTypes.BUY_ASSET,
                ActionType = m_context.TransactionActionTypes.First(x => x.Code == TransactionActionTypes.BUY_ASSET)
            });

            if (request.Expenses > 0)
            {
                transactionGroup.Transactions.Add(new BankAccountTransaction
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now.ToUniversalTime(),
                    Quantity = 1M,
                    InOut = InOut.Outgoing,
                    Price = request.Expenses,
                    ActionTypeCode = TransactionActionTypes.BUY_ASSET_FEE,
                    ActionType =
                        m_context.TransactionActionTypes.First(x => x.Code == TransactionActionTypes.BUY_ASSET_FEE)
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