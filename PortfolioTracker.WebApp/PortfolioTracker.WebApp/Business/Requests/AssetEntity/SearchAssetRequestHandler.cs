using System.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Extensions;

namespace PortfolioTracker.WebApp.Business.Requests.AssetEntity;

public sealed class AssetModel
{
    public Guid Id { get; init; }

    public string TickerSymbol { get; init; } = string.Empty;

    public string ExchangeCode { get; init; } = string.Empty;

    public string ExchangeName { get; init; } = string.Empty;

    public string ExchangeCountryCode { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;

    public string CurrencyName { get; init; } = string.Empty;

    public string CurrencySymbol { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;

    public string Isin { get; init; } = string.Empty;

    public string Wkn { get; init; } = string.Empty;
    
    public string WebSite { get; init; } = string.Empty;

    public DateTime Created { get; init; }

    public DateTime? Updated { get; init; }

    public decimal Price { get; init; }
}

public sealed class SearchAssetRequest : IRequest<IEnumerable<AssetModel>>
{
    public AssetTypes? AssetType { get; init; } = null;
    
    public string? SearchText { get; init; } = string.Empty;

    public int PageIndex { get; init; } = 0;

    public int PageSize { get; init; } = 20;
}

public sealed class
    SearchAssetRequestHandler : IRequestHandler<SearchAssetRequest, IEnumerable<AssetModel>>
{
    private readonly IPortfolioContext m_context;

    public SearchAssetRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<AssetModel>> Handle(SearchAssetRequest request,
        CancellationToken cancellationToken)
    {
        var searchString = string.IsNullOrEmpty(request.SearchText) ? string.Empty : request.SearchText.ToLower();

        var result = await m_context
            .Assets
            .Include(x => x.Exchange)
            .Include(x => x.Currency)
            .Where(x => !request.AssetType.HasValue || x.AssetType == request.AssetType)
            .Where(x =>
                string.IsNullOrEmpty(searchString) ||
                (x.Name.ToLower().Contains(searchString) ||
                 x.TickerSymbol.ToLower().Contains(searchString) ||
                 x.Isin.ToLower().Contains(searchString) ||
                 x.Wkn.ToLower().Contains(searchString)))
            .OrderByDescending(x => x.Created)
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return result.Select(x => new AssetModel
        {
            Id = x.Id,
            TickerSymbol = x.TickerSymbol,
            ExchangeCode = x.Exchange.GetCode(),
            ExchangeName = x.Exchange.GetName(),
            ExchangeCountryCode = x.Exchange.CountryCode,
            CurrencyCode = x.CurrencyCode,
            CurrencyName = x.Currency.Name,
            CurrencySymbol = x.Currency.Symbol,
            Name = x.Name,
            Isin = x.Isin,
            Wkn = x.Wkn,
            Created = x.Created,
            Updated = x.Updated,
            Price = x.Price
        });
    }
}