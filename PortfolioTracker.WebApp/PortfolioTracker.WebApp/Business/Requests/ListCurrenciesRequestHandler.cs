using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class CurrencyInfoModel
{
    public string Code { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Symbol { get; set; } = string.Empty;
}

public sealed class ListCurrenciesRequest : IRequest<IEnumerable<CurrencyInfoModel>>
{
}

public class ListCurrenciesRequestHandler : IRequestHandler<ListCurrenciesRequest, IEnumerable<CurrencyInfoModel>>
{
    private readonly PortfolioContext m_context;

    public ListCurrenciesRequestHandler(PortfolioContext mContext)
    {
        m_context = mContext;
    }

    public async Task<IEnumerable<CurrencyInfoModel>> Handle(ListCurrenciesRequest request, CancellationToken cancellationToken)
    {
        return await m_context.Currencies.Select(x => new CurrencyInfoModel
        {
            Code = x.Code,
            Name = x.Name,
            Symbol = x.Symbol
        }).ToListAsync(cancellationToken: cancellationToken);
    }
}