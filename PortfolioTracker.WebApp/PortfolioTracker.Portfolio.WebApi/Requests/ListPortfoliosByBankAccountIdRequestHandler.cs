using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Portfolio.WebApi.Models;

namespace PortfolioTracker.Portfolio.WebApi.Requests;

public static class ListPortfoliosByBankAccountIdEndpointExtensions
{
    public static void MapListPortfoliosByBankAccountId(this IEndpointRouteBuilder app)
    {
        app.MapGet("/bybankaccount/{bankAccountId}", ListPortfoliosByBankAccountId)
           .WithName(nameof(ListPortfoliosByBankAccountId));
    }

    private static async Task<IResult> ListPortfoliosByBankAccountId(IMediator mediator, Guid bankAccountId)
    {
        var result = await mediator.Send(new ListPortfoliosByBankAccountIdRequest { BankAccountId = bankAccountId });
        return TypedResults.Ok(result);
    }
}

//public sealed class PortfolioModel
//{
//    public Guid Id { get; init; }

//    public string Name { get; init; } = string.Empty;

//    public string Description { get; init; } = string.Empty;

//    public bool IsDefault { get; init; }

//    public DateTime Created { get; init; }
    
//    public Guid BankAccountId { get; init; }
    
//    public string BankAccountName { get; init; } = string.Empty;
    
//    public string CurrencyCode { get; init; } = string.Empty;
    
//    public string CurrencyName { get; init; } = string.Empty;
    
//    public string CurrencySymbol { get; init; } = string.Empty;
//}

public sealed class ListPortfoliosByBankAccountIdRequest : IRequest<IEnumerable<PortfolioModel>>
{
    public Guid BankAccountId { get; init; }
}

public sealed class ListPortfoliosByBankAccountIdRequestHandler : IRequestHandler<ListPortfoliosByBankAccountIdRequest, IEnumerable<PortfolioModel>>
{
    private readonly IPortfolioContext m_context;

    public ListPortfoliosByBankAccountIdRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<PortfolioModel>> Handle(ListPortfoliosByBankAccountIdRequest request,
        CancellationToken cancellationToken)
    {
        return await m_context
            .Portfolios
            .Include(p => p.BankAccount)
            .ThenInclude(bankAccount => bankAccount!.Currency)
            .Where(p => p.BankAccountId == request.BankAccountId)
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
