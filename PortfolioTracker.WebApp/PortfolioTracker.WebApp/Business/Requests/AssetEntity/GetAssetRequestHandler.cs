using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Extensions;

namespace PortfolioTracker.WebApp.Business.Requests.AssetEntity;

public sealed class GetAssetRequest : IRequest<AssetModel?>
{
    public Guid Id { get; init; }
}

public sealed class GetAssetRequestHandler : IRequestHandler<GetAssetRequest, AssetModel?>
{
    private readonly IPortfolioContext m_context;

    public GetAssetRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<AssetModel?> Handle(GetAssetRequest request, CancellationToken cancellationToken)
    {
        var assetItem = await m_context
            .Assets
            .Include(x => x.Exchange)
            .Include(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (assetItem == null)
        {
            return null;
        }

        return new AssetModel
        {
            Id = assetItem.Id,
            TickerSymbol = assetItem.TickerSymbol,
            ExchangeCode = assetItem.Exchange.GetCode(),
            ExchangeName = assetItem.Exchange.GetName(), 
            ExchangeCountryCode = assetItem.Exchange.CountryCode,
            CurrencyCode = assetItem.CurrencyCode,
            CurrencyName = assetItem.Currency.Name,
            CurrencySymbol = assetItem.Currency.Symbol,
            Name = assetItem.Name,
            Description = assetItem.Description,
            Isin = assetItem.Isin,
            Wkn = assetItem.Wkn,
            WebSite = assetItem.WebSite,
            Created = assetItem.Created,
            Updated = assetItem.Updated,
            Price = assetItem.Price
        };
    }
}
