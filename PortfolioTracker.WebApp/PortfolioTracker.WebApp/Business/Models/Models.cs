using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Models;

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

public sealed class HoldingAggregateModel
{
    public Guid PortfolioId { get; init; }
    
    public Guid AssetId { get; init; }

    public required string AssetName { get; init; }

    public required string AssetTickerSymbol { get; init; }

    public decimal AssetPrice { get; init; }
    
    public AssetTypes AssetType { get; init; }
    
    public required string AssetTypeName { get; init; } 
    
    public required string ExchangeCode { get; init; }
    
    public required string CountryCode { get; init; }
    
    public required string CurrencyCode { get; init; }
    
    public required string CurrencyName { get; init; }

    public required string CurrencySymbol { get; init; }
    
    public decimal TotalQuantity { get; init; }
    
    public decimal TotalCost { get; init; }
    
    public decimal AveragePrice { get; init; }
    
    public decimal TotalExpenses { get; init; }
}
