using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Portfolio.WebApi.Models;

namespace PortfolioTracker.Portfolio.WebApi.Requests;

public static class GetPortfolioByIdEndpointExtensions
{
    public static void MapGetPortfolioById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", GetPortfolioById)
           .WithName(nameof(GetPortfolioById));
    }

    private static async Task<IResult> GetPortfolioById(IMediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetPortfolioByIdRequest { PortfolioId = id });
        
        if (result is null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(result);
    }
}

public sealed class GetPortfolioByIdRequest : IRequest<PortfolioModel?>
{
    public Guid PortfolioId { get; init; }
}

public sealed class GetPortfolioByIdRequestHandler : IRequestHandler<GetPortfolioByIdRequest, PortfolioModel?>
{
    private readonly IPortfolioContext m_context;

    public GetPortfolioByIdRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<PortfolioModel?> Handle(GetPortfolioByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await m_context
            .Portfolios
            .Include(x => x.BankAccount)
            .ThenInclude(bankAccount => bankAccount!.Currency)
            .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

        if (result is null)
        {
            return null;
        }

        return new PortfolioModel
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            IsDefault = result.IsDefault,
            Created = result.Created,
            BankAccountId = result.BankAccountId,
            BankAccountName = result.BankAccount!.Name,
            CurrencyCode = result.BankAccount.CurrencyCode,
            CurrencyName = result.BankAccount.Currency.Name,
            CurrencySymbol = result.BankAccount.Currency.Symbol,
        };
    }
}
