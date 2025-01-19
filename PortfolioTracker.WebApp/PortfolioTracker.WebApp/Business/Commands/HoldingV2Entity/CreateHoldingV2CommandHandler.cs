using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.HoldingV2Entity;

public sealed class CreateHoldingV2Command : IRequest<bool>
{
    public Guid Id { get; set; }

    public Guid PortfolioId { get; set; }

    public Guid AssetId { get; set; }
}

public sealed class CreateHoldingV2CommandHandler : IRequestHandler<CreateHoldingV2Command, bool>
{
    private readonly PortfolioContext m_context;

    public CreateHoldingV2CommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreateHoldingV2Command request, CancellationToken cancellationToken)
    {
        try
        {
            var portfolio = await m_context
                .PortfolioV2s
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            var asset = await m_context.Assets.FirstOrDefaultAsync(x => x.Id == request.AssetId,
                cancellationToken: cancellationToken);

            if (portfolio == null || asset == null)
            {
                return false;
            }

            var transactionGroup = new TransactionGroup
            {
                Id = Guid.NewGuid(),
            };

            portfolio.Holdings.Add(new HoldingV2
            {
                Id = request.Id,
                PortfolioId = request.PortfolioId,
                Portfolio = portfolio,
                AssetId = asset.Id,
                Asset = asset,
                TransactionGroupId = transactionGroup.Id, 
                TransactionGroup = transactionGroup,
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