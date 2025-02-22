using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.TransactionActionTypeEntity;

public sealed class ListTransactionActionTypesRequest : IRequest<IEnumerable<TransactionActionType>>
{
    
}

public sealed class ListTransactionActionTypesRequestHandler : IRequestHandler<ListTransactionActionTypesRequest, IEnumerable<TransactionActionType>>
{
    private readonly PortfolioContext m_context;

    public ListTransactionActionTypesRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<TransactionActionType>> Handle(ListTransactionActionTypesRequest request, CancellationToken cancellationToken)
    {
        return await m_context.TransactionActionTypes.ToListAsync(cancellationToken);
    }
}