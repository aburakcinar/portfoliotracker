using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Portfolio.WebApi.Models;

namespace PortfolioTracker.Portfolio.WebApi.Requests;

public static class ListAllPortfoliosEndpointExtensions
{
    public static void MapListAllPortfolios(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", ListAllPortfolios)
           .WithName(nameof(ListAllPortfolios));
    }

    private static async Task<IResult> ListAllPortfolios(IMediator mediator)
    {
        var result = await mediator.Send(new ListAllPortfoliosRequest());
        return TypedResults.Ok(result);
    }
}

public sealed class ListAllPortfoliosRequest : IRequest<IEnumerable<PortfolioModel>>
{
}

public sealed class ListAllPortfoliosRequestHandler : IRequestHandler<ListAllPortfoliosRequest, IEnumerable<PortfolioModel>>
{
    private readonly IPortfolioContext m_context;

    public ListAllPortfoliosRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<PortfolioModel>> Handle(ListAllPortfoliosRequest request,
        CancellationToken cancellationToken)
    {
        return await m_context
            .Portfolios
            .Include(p => p.BankAccount)
            .ThenInclude(bankAccount => bankAccount!.Currency)
            .Select(x => new PortfolioModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsDefault = x.IsDefault,
                Created = x.Created,
                BankAccountId = x.BankAccountId,
                BankAccountName = x.BankAccount!.Name,
                CurrencyCode = x.BankAccount.CurrencyCode,
                CurrencyName = x.BankAccount.Currency.Name,
                CurrencySymbol = x.BankAccount.Currency.Symbol,
            }).ToListAsync(cancellationToken);
    }
}
