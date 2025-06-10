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

//public sealed class BankTransactionModel
//{
//    public Guid Id { get; init; }

//    public decimal Price { get; set; }

//    public decimal Quantity { get; set; }

//    public InOut InOut { get; set; }

//    public string ActionTypeCode { get; set; } = string.Empty;
//}

//public sealed class BankTransactionGroupModel
//{
//    public Guid Id { get; init; }

//    public Guid BankAccountId { get; init; }

//    public DateTime OperationDate { get; init; }

//    public string Description { get; init; } = string.Empty;

//    public Decimal Amount { get; set; }

//    public decimal Balance { get; set; }

//    public List<BankTransactionModel> Transactions { get; init; } = new();
//}

// Request
public sealed class ListBankAccountTransactionsRequest : IRequest<IEnumerable<BankAccountTransactionGroupModel>>
{
    public Guid BankAccountId { get; init; }
}

public sealed class ListBankAccountTransactionsRequestHandler : IRequestHandler<ListBankAccountTransactionsRequest, IEnumerable<BankAccountTransactionGroupModel>>
{
    private readonly IPortfolioContext m_context;

    public ListBankAccountTransactionsRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<BankAccountTransactionGroupModel>> Handle(ListBankAccountTransactionsRequest request, CancellationToken cancellationToken)
    {
        var entities = await m_context
            .BankAccountTransactionGroups
            .Include(x => x.Transactions)
            .Where(x => x.BankAccountId == request.BankAccountId)
            .ToListAsync(cancellationToken);

        var result = entities
            .Select(ToBankTransactionGroupModel)
            .OrderBy(x => x.ExecuteDate)
            .ToList();

        return result.OrderByDescending(x => x.ExecuteDate).ToList();
    }

    private BankAccountTransactionGroupModel ToBankTransactionGroupModel(BankAccountTransactionGroup item)
    {
        return new BankAccountTransactionGroupModel
        {
            Id = item.Id,
            BankAccountId = item.BankAccountId,
            Transactions = item.Transactions.Select(ToBankTransactionModel).ToList(),
            Description = item.Description
        };
    }

    private BankAccountTransactionModel ToBankTransactionModel(BankAccountTransaction transaction)
    {
        return new BankAccountTransactionModel
        {
            Id = transaction.Id,
            Price = transaction.Price,
            Quantity = transaction.Quantity,
            InOut = transaction.InOut,            
        };
    }
}
