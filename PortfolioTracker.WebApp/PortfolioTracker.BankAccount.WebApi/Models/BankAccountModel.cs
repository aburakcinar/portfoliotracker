namespace PortfolioTracker.BankAccount.WebApi.Models;

public sealed class BankAccountModel
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string BankName { get; init; }

    public required string AccountHolder { get; init; }

    public required string Description { get; init; }

    public required string Iban { get; init; }

    public required string CurrencyCode { get; init; }

    public required string CurrencyName { get; init; }

    public required string CurrencyNameLocal { get; init; }

    public required string CurrencySymbol { get; init; }

    public required string LocaleCode { get; init; }

    public required string CountryName { get; init; }

    public required string CountryNameLocal { get; init; }

    public required string CountryCode { get; init; }

    public DateTime OpenDate { get; init; }

    public DateTime Created { get; init; }
}
