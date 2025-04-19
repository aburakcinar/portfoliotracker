using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests.PortfolioV2Entity;

public sealed class GetPortfolioRequest : IRequest<PortfolioModel?>
{
    public Guid PortfolioId { get; init; }
}

public sealed class GetPortfolioRequestHandler : IRequestHandler<GetPortfolioRequest, PortfolioModel?>
{
    private readonly IPortfolioContext m_context;

    public GetPortfolioRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<PortfolioModel?> Handle(GetPortfolioRequest request, CancellationToken cancellationToken)
    {
        var result = await m_context
            .Portfolios
            .Include(x => x.BankAccount)
            .ThenInclude(bankAccount => bankAccount.Currency)
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
            CurrencyCode = result.BankAccount.CurrencyCode,
            CurrencyName = result.BankAccount.Currency.Name,
            CurrencySymbol = result.BankAccount.Currency.Symbol,
        };
    }
}