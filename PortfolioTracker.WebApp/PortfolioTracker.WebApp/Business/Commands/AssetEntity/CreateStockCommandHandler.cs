using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.AssetEntity;

public sealed class CreateAssetStockCommand : IRequest<bool>
{
    public string TickerSymbol { get; init; } = string.Empty;
    
    public string ExchangeCode { get; init; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
    
    public string Isin { get; init; } = string.Empty;
    
    public string Wkn { get; init; } = string.Empty;
    
    public string WebSite { get; init; } = string.Empty;
}

public sealed class CreateAssetStockCommandHandler : IRequestHandler<CreateAssetStockCommand, bool>
{
    private readonly PortfolioContext m_context;

    public CreateAssetStockCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreateAssetStockCommand request, CancellationToken cancellationToken)
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
                TickerSymbol = request.TickerSymbol,
                ExchangeCode = exchange.Mic,
                Exchange = exchange,
                CurrencyCode = currency.Code,
                Currency = currency,
                AssetType = AssetTypes.Stock,
                Name = request.Name,
                Description = request.Description,
                Isin = request.Isin,
                Wkn = request.Wkn,
                Created = DateTime.Now.ToUniversalTime(),
                Price = 0
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