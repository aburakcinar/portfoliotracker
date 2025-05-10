using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.BankAccount.WebApi.Requests;

public static class ImportBankAccountsEndpointExtension
{
    public static void MapImportBankAccounts(this IEndpointRouteBuilder app)
    {
        app.MapPost(@"/import", ImportBankAccounts)
            .WithName(nameof(ImportBankAccounts));
    }

    private static async Task<IResult> ImportBankAccounts(
        [FromBody] ImportBankAccountsCommand command,
        IMediator mediator
        )
    {
        var result = await mediator.Send(command);
        return result ? TypedResults.Ok() : TypedResults.BadRequest();
    }
}

public sealed class ImportBankAccountItem
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string BankName { get; init; }

    public required string AccountHolder { get; init; }

    public required string Description { get; init; }

    public required string Iban { get; init; }

    public required string CurrencyCode { get; init; }

    public required string LocaleCode { get; init; }

    public DateTime OpenDate { get; init; }
}

public sealed class ImportBankAccountsCommand : IRequest<bool>
{
    public ImportBankAccountItem[] Items { get; init; }

    public bool Override { get; init; } = false;
}

public sealed class ImportBankAccountsCommandHandler : IRequestHandler<ImportBankAccountsCommand, bool>
{
    private readonly IPortfolioContext m_context;

    public ImportBankAccountsCommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(ImportBankAccountsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var item in request.Items)
            {
                var found = await m_context.BankAccounts.FirstOrDefaultAsync(x => x.Id == item.Id, cancellationToken);
                if (found != null)
                {
                    if (request.Override)
                    {
                        found.Name = item.Name;
                        found.BankName = item.BankName;
                        found.AccountHolder = item.AccountHolder;
                        found.Description = item.Description;
                        found.Iban = item.Iban;
                        found.CurrencyCode = item.CurrencyCode;
                        found.LocaleCode = item.LocaleCode;
                        found.OpenDate = item.OpenDate;
                        m_context.BankAccounts.Update(found);
                    }
                }
                else
                {
                    var newItem = new Data.Models.BankAccount
                    {
                        Id = item.Id,
                        Name = item.Name,
                        BankName = item.BankName,
                        AccountHolder = item.AccountHolder,
                        Description = item.Description,
                        Iban = item.Iban,
                        CurrencyCode = item.CurrencyCode,
                        LocaleCode = item.LocaleCode,
                        OpenDate = item.OpenDate
                    };
                    await m_context.BankAccounts.AddAsync(newItem, cancellationToken);
                }
            }

            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
