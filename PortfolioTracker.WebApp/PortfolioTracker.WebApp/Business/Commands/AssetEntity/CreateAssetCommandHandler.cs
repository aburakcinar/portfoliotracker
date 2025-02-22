using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.AssetEntity;

public sealed class CreateAssetCommand : IRequest<bool>
{
    public AssetTypes AssetType { get; init; }
    
    public string TickerSymbol { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
    
    public string ExchangeCode { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;
}

public sealed class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, bool>
{
    private readonly PortfolioContext m_context;

    public CreateAssetCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exchange = await m_context.Exchanges.FirstOrDefaultAsync(x => x.Mic == request.ExchangeCode, cancellationToken);
            var currency = await m_context.Currencies.FirstOrDefaultAsync(x => x.Code == request.CurrencyCode, cancellationToken);

            if (exchange is null || currency is null)
            {
                return false;
            }
            
            m_context.Assets.Add(new Asset
            {
                Id = Guid.NewGuid(),
                AssetType = request.AssetType,
                TickerSymbol = request.TickerSymbol,
                ExchangeCode = exchange.Mic,
                Exchange = exchange,
                CurrencyCode = currency.Code,
                Currency = currency,
                Name = request.Name,
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