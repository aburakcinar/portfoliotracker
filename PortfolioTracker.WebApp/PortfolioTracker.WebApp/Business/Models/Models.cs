using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Models;

public sealed class LocaleQueryModel
{
    public required string LocaleCode { get; init; }

    public required string LanguageName { get; init; }

    public required string CountryName { get; init; }

    public required string CountryCode { get; init; }

    public required string CurrencyCode { get; set; }

    public required string CurrencyName { get; set; }

    public required string CurrencySymbol { get; set; }
}
