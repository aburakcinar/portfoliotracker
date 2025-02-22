using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Business.Commands.AssetEntity;

public sealed class ImportAssetCommand : IRequest<bool>
{
    public required AssetItem AssetItem { get; set; }
}

public sealed class ImportAssetCommandHandler : IRequestHandler<ImportAssetCommand, bool>
{
    private readonly PortfolioContext m_context;

    public ImportAssetCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(ImportAssetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var found  = await m_context.Assets.FirstOrDefaultAsync(x => x.Id == request.AssetItem.Id, cancellationToken);

            if (found is not null)
            {
                return false;
            }
            
            var exchange = await m_context.Exchanges.FirstOrDefaultAsync(x => x.Mic == request.AssetItem.ExchangeCode, cancellationToken);
            var currency = await m_context.Currencies.FirstOrDefaultAsync(x => x.Code == request.AssetItem.CurrencyCode, cancellationToken);

            if (exchange is null || currency is null)
            {
                return false;
            }
            
            var assetItem = request.AssetItem;
            
            m_context.Assets.Add(new Asset
            {
                Id = assetItem.Id,
                TickerSymbol = assetItem.TickerSymbol,
                ExchangeCode = exchange.Mic,
                Exchange = exchange,
                CurrencyCode = currency.Code,
                Currency = currency,
                AssetType = assetItem.AssetType,
                Name = assetItem.Name,
                Description = assetItem.Description,
                Isin = assetItem.Isin,
                Wkn = assetItem.Wkn,
                Created = DateTime.Now.ToUniversalTime(),
                WebSite = assetItem.WebSite,
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