using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class CurrencyInfoModel
{
    public required string Code { get; set; } 
    
    public required string Name { get; set; } 
    
    public required string NameLocal { get; set; }

    public required string Symbol { get; set; }

    public int SubunitValue { get; set; }

    public required string SubunitName { get; set; }
}

public sealed class ListCurrenciesRequest : IRequest<IEnumerable<CurrencyInfoModel>>
{
}

public class ListCurrenciesRequestHandler : IRequestHandler<ListCurrenciesRequest, IEnumerable<CurrencyInfoModel>>
{
    private readonly IPortfolioContext m_context;

    public ListCurrenciesRequestHandler(IPortfolioContext mContext)
    {
        m_context = mContext;
    }

    public async Task<IEnumerable<CurrencyInfoModel>> Handle(ListCurrenciesRequest request, CancellationToken cancellationToken)
    {
        return await m_context.Currencies.Select(x => new CurrencyInfoModel
        {
            Code = x.Code,
            Name = x.Name,
            NameLocal = x.NameLocal,
            Symbol = x.Symbol,
            SubunitValue = x.SubunitValue,
            SubunitName = x.SubunitName
        }).ToListAsync(cancellationToken: cancellationToken);
    }
}