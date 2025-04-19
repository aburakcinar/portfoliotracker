using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests.TransactionActionTypeEntity;

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