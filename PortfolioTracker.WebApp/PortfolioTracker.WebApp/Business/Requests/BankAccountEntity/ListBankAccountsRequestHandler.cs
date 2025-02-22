using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.BankAccountEntity;

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

public sealed class ListBankAccountsRequest : IRequest<IEnumerable<BankAccountModel>>
{
}

public sealed class ListBankAccountsRequestHandler : IRequestHandler<ListBankAccountsRequest, IEnumerable<BankAccountModel>>
{
    private readonly PortfolioContext m_context;

    public ListBankAccountsRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<BankAccountModel>> Handle(ListBankAccountsRequest request, CancellationToken cancellationToken)
    {
        return await m_context
            .BankAccounts
            .Include(x => x.Currency)
            .Include(x => x.Locale)
            .Select(x => new BankAccountModel
            {
                Id = x.Id,
                Name = x.Name,
                BankName = x.BankName,
                AccountHolder = x.AccountHolder,
                Description = x.Description,
                Iban = x.Iban,
                CurrencyCode = x.CurrencyCode,
                CurrencyName = x.Currency.Name,
                CurrencyNameLocal = x.Currency.NameLocal,
                CurrencySymbol = x.Currency.Symbol,
                LocaleCode = x.LocaleCode,
                CountryName = x.Locale.CountryName,
                CountryNameLocal = x.Locale.CountryNameLocal,
                CountryCode = x.Locale.CountryCode,
                OpenDate = x.OpenDate,
                Created = x.Created
            })
            .ToListAsync(cancellationToken);
    }
}