namespace PortfolioTracker.Portfolio.WebApi.Models;


public sealed class PortfolioModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public bool IsDefault { get; init; }

    public DateTime Created { get; init; }

    public Guid BankAccountId { get; init; }

    public string BankAccountName { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;

    public string CurrencyName { get; init; } = string.Empty;

    public string CurrencySymbol { get; init; } = string.Empty;
}

