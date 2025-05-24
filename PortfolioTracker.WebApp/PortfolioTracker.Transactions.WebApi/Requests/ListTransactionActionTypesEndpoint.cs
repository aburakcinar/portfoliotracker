using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Transactions.WebApi.Requests;

// Router extension
public static class ListTransactionActionTypesEndpoint
{
    public static IEndpointRouteBuilder MapListTransactionActionTypesEndpoint(this IEndpointRouteBuilder group)
    {
        group.MapGet(@"/actiontypes", async (IMediator mediator) =>
            await mediator.Send(new ListTransactionActionTypesRequest()))
            .WithName(@"ListActionTypes")
            .WithTags(@"Transactions");
        return group;
    }
}

public sealed class ListTransactionActionTypesRequest : IRequest<IEnumerable<TransactionActionType>>
{

}

public sealed class ListTransactionActionTypesRequestHandler : IRequestHandler<ListTransactionActionTypesRequest, IEnumerable<TransactionActionType>>
{
    private readonly IPortfolioContext m_context;

    public ListTransactionActionTypesRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<TransactionActionType>> Handle(ListTransactionActionTypesRequest request, CancellationToken cancellationToken)
    {
        return await m_context.TransactionActionTypes.ToListAsync(cancellationToken);
    }
}