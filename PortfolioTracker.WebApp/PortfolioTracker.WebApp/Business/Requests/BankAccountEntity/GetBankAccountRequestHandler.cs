using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests.BankAccountEntity;

public sealed class GetBankAccountRequest : IRequest<BankAccountModel?>
{
    public Guid Id { get; init; }
}

public sealed class GetBankAccountRequestHandler : IRequestHandler<GetBankAccountRequest, BankAccountModel?>
{
    private readonly IPortfolioContext m_context;

    public GetBankAccountRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<BankAccountModel?> Handle(GetBankAccountRequest request, CancellationToken cancellationToken)
    {
        var item = await m_context
            .BankAccounts
            .Include(x => x.Currency)
            .Include(x => x.Locale)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (item is null)
        {
            return null;
        }
        
        return new BankAccountModel
        {
            Id = item.Id,
            Name = item.Name,
            BankName = item.BankName,
            AccountHolder = item.AccountHolder,
            Description = item.Description,
            Iban = item.Iban,
            CurrencyCode = item.CurrencyCode,
            CurrencyName = item.Currency.Name,
            CurrencyNameLocal = item.Currency.NameLocal,
            CurrencySymbol = item.Currency.Symbol,
            LocaleCode = item.LocaleCode,
            CountryName = item.Locale.CountryName,
            CountryNameLocal = item.Locale.CountryNameLocal,
            CountryCode = item.Locale.CountryCode,
            OpenDate = item.OpenDate,
            Created = item.Created
        };
    }
}