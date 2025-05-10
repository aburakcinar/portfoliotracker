namespace PortfolioTracker.Assets.WebApi.Models;

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

