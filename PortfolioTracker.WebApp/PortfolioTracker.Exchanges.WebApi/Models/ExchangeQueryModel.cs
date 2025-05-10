namespace PortfolioTracker.Exchanges.WebApi.Models;

public sealed class ExchangeQueryModel
{
    public required string Code { get; init; }

    public required string Mic { get; init; }

    public required string MarketNameInstitutionDescription { get; init; }

    public required string LegalEntityName { get; init; }

    public required string CountryCode { get; init; }

    public required string City { get; init; }

    public required string CurrencyCode { get; init; }
}
