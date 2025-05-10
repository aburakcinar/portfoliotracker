using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.BankAccount.WebApi.Models;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.BankAccount.WebApi.Requests;

public static class ListBankAccountEndpointExtension
{
    public static void MapListBankAccounts(this IEndpointRouteBuilder app)
    {
        app.MapGet(@"/", ListBankAccounts)
            .WithName(nameof(ListBankAccounts));
    }

    private static async Task<IResult> ListBankAccounts(IMediator mediator)
    {
        var result = await mediator.Send(new ListBankAccountsRequest());
        return TypedResults.Ok(result);
    }
}

public sealed class ListBankAccountsRequest : IRequest<IEnumerable<BankAccountModel>>
{
}

public sealed class ListBankAccountsRequestHandler : IRequestHandler<ListBankAccountsRequest, IEnumerable<BankAccountModel>>
{
    private readonly IPortfolioContext m_context;

    public ListBankAccountsRequestHandler(IPortfolioContext context)
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