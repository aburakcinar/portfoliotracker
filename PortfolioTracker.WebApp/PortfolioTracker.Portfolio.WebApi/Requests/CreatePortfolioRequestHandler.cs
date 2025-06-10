using MediatR;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Portfolio.WebApi.Requests;

public static class CreatePortfolioEndpointExtensions
{
    public static void MapCreatePortfolio(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", CreatePortfolio).WithName(nameof(CreatePortfolio));
    }

    private static async Task<IResult> CreatePortfolio(
        IMediator mediator,
        CreatePortfolioRequest request
    )
    {
        var result = await mediator.Send(request);

        if (result)
        {
            return TypedResults.Created();
        }

        return TypedResults.BadRequest();
    }
}

public sealed class CreatePortfolioRequest : IRequest<bool>
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public Guid BankAccountId { get; init; }
}

public sealed class CreatePortfolioRequestHandler : IRequestHandler<CreatePortfolioRequest, bool>
{
    private readonly IPortfolioContext m_context;

    public CreatePortfolioRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(
        CreatePortfolioRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            m_context.Portfolios.Add(
                new Data.Models.Portfolio
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    BankAccountId = request.BankAccountId,
                    Created = DateTime.UtcNow,
                    IsDefault = false,
                }
            );

            var count = await m_context.SaveChangesAsync(cancellationToken);

            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}
