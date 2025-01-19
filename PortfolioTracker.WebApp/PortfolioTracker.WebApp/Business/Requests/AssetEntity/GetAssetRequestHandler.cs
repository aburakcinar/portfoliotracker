using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.AssetEntity;

public sealed class GetAssetRequest : IRequest<AssetModel?>
{
    public Guid Id { get; init; }
}

public sealed class GetAssetRequestHandler : IRequestHandler<GetAssetRequest, AssetModel?>
{
    private readonly PortfolioContext m_context;

    public GetAssetRequestHandler(PortfolioContext context)
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
            ExchangeCode = assetItem.ExchangeCode,
            ExchangeName = assetItem.Exchange?.MarketNameInstitutionDescription ?? string.Empty, 
            ExchangeCountryCode = assetItem.Exchange?.CountryCode ?? string.Empty,
            CurrencyCode = assetItem.CurrencyCode,
            CurrencyName = assetItem.Currency?.Name ?? string.Empty,
            CurrencySymbol = assetItem.Currency?.Symbol ?? string.Empty,
            Name = assetItem.Name,
            Description = assetItem.Description,
            Isin = assetItem.Isin,
            Wkn = assetItem.Wkn,
            Created = assetItem.Created,
            Updated = assetItem.Updated,
            Price = assetItem.Price
        };
    }
}
