using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Transactions.WebApi.Requests;

// Router extension
public static class AddTransactionEndpoint
{
    public static IEndpointRouteBuilder MapAddTransactionEndpoint(this IEndpointRouteBuilder group)
    {
        group.MapPost(@"/", async (AddTransactionCommand command, IMediator mediator) =>
            await mediator.Send(command))
            .WithName(@"AddTransaction")
            .WithTags(@"Transactions");
        return group;
    }
}

public class TransactionCreateModel
{
    public decimal Price { get; init; }

    public decimal Quantity { get; init; }

    public InOut InOut { get; init; }

    public string ActionTypeCode { get; init; } = string.Empty;
}

public sealed class AddTransactionCommand : IRequest<bool>
{
    public Guid BankAccountId { get; init; }

    public DateTime OperationDate { get; init; }

    public List<TransactionCreateModel> Transactions { get; init; } = new();
}

public sealed class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, bool>
{
    private readonly IPortfolioContext m_context;

    public AddTransactionCommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var bankAccount = await m_context
                .BankAccounts
                .FirstOrDefaultAsync(x => x.Id == request.BankAccountId, cancellationToken);

            if (bankAccount is null)
            {
                return false;
            }

            var transactionGroup = new BankAccountTransactionGroup
            {
                Id = Guid.NewGuid(),
                BankAccountId = bankAccount.Id,
            };

            foreach (var item in request.Transactions)
            {
                transactionGroup.Transactions.Add(new BankAccountTransaction
                {
                    Id = Guid.NewGuid(),
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Created = request.OperationDate.ToUniversalTime(),
                    InOut = item.InOut,
                    ActionTypeCode = item.ActionTypeCode,
                    Description = string.Empty, // TODO : Fill description
                    BankAccountTransactionGroupId = transactionGroup.Id
                });
            }

            m_context.BankAccountTransactionGroups.Add(transactionGroup);

            var result = await m_context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        catch
        {
            return false;
        }
    }
}

