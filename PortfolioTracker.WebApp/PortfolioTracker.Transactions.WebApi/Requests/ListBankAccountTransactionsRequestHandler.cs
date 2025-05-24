using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Transactions.WebApi.Requests;


// Router extension
public static class ListBankAccountTransactionsEndpoint
{
    public static IEndpointRouteBuilder MapListBankAccountTransactionsEndpoint(this IEndpointRouteBuilder group)
    {
        group.MapGet(@"/listbybankaccount/{bankAccountId}", async (Guid bankAccountId, IMediator mediator) =>
            await mediator.Send(new ListBankAccountTransactionsRequest { BankAccountId = bankAccountId }))
            .WithName(@"ListTransactionsByBankAccount")
            .WithTags(@"Transactions");
        return group;
    }
}

public sealed class BankTransactionModel
{
    public Guid Id { get; init; }

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }

    public InOut InOut { get; set; }

    public string ActionTypeCode { get; set; } = string.Empty;
}

public sealed class BankTransactionGroupModel
{
    public Guid Id { get; init; }

    public Guid BankAccountId { get; init; }

    public DateTime OperationDate { get; init; }

    public string Description { get; init; } = string.Empty;

    public Decimal Amount { get; set; }

    public decimal Balance { get; set; }

    public List<BankTransactionModel> Transactions { get; init; } = new();
}

// Request
public sealed class ListBankAccountTransactionsRequest : IRequest<IEnumerable<BankTransactionGroupModel>>
{
    public Guid BankAccountId { get; init; }
}

public sealed class ListBankAccountTransactionsRequestHandler : IRequestHandler<ListBankAccountTransactionsRequest, IEnumerable<BankTransactionGroupModel>>
{
    private readonly IPortfolioContext m_context;

    public ListBankAccountTransactionsRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<BankTransactionGroupModel>> Handle(ListBankAccountTransactionsRequest request, CancellationToken cancellationToken)
    {
        var entities = await m_context
            .BankAccountTransactionGroups
            .Include(x => x.Transactions)
            .ThenInclude(transaction => transaction.ActionType)
            .Where(x => x.BankAccountId == request.BankAccountId)
            .ToListAsync(cancellationToken);

        var result = entities
            .Select(ToBankTransactionGroupModel)
            .OrderBy(x => x.OperationDate)
            .ToList();

        var balance = 0M;

        foreach (var transaction in result)
        {
            balance += transaction.Amount;

            transaction.Balance = balance;
        }

        return result.OrderByDescending(x => x.OperationDate).ToList();
    }

    private BankTransactionGroupModel ToBankTransactionGroupModel(BankAccountTransactionGroup item)
    {
        return new BankTransactionGroupModel
        {
            Id = item.Id,
            BankAccountId = item.BankAccountId,
            Amount = item.Transactions.Sum(x => (x.InOut == InOut.Outgoing ? -1 : 1) * x.Quantity * x.Price),
            OperationDate = item.Transactions.First().Created,
            Transactions = item.Transactions.Select(ToBankTransactionModel).ToList(),
            Description = item.Description
        };
    }

    private BankTransactionModel ToBankTransactionModel(BankAccountTransaction transaction)
    {
        return new BankTransactionModel
        {
            Id = transaction.Id,
            Price = transaction.Price,
            Quantity = transaction.Quantity,
            InOut = transaction.InOut,
            ActionTypeCode = transaction.ActionTypeCode
        };
    }
}
