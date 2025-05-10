using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Assets.WebApi.Models;
using PortfolioTracker.Assets.WebApp.Extensions;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Assets.WebApp.Requests;

public static class GetAssetRequestExtensions
{
    public static void MapGetAsset(this IEndpointRouteBuilder app)
    {
        app.MapGet(@"/{id:guid}", GetAsset);
    }
    public static async Task<IResult> GetAsset(
        IMediator mediator,
        Guid id)
    {
        var result = await mediator.Send(new GetAssetRequest
        {
            Id = id
        });
        return TypedResults.Ok(result);
    }
}

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
