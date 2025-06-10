using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Imports.WebApi.Services;

namespace PortfolioTracker.Imports.WebApi.Requests;

public static class ImportTransactionsFromFileEndpointExtension
{
    public static void MapImportTransactionsFromFile(this IEndpointRouteBuilder app)
    {
        app.MapPost(@"/import-transactions", ImportTransactionsFromFile)
            .WithName(nameof(ImportTransactionsFromFile))
            .DisableAntiforgery();
    }

    private static async Task<IResult> ImportTransactionsFromFile(
        [FromForm] ImportTransactionsFromFileCommand command,
        IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result > 0 ? TypedResults.Ok(result) : TypedResults.BadRequest();
    }
}

public sealed class ImportTransactionsFromFileCommand : IRequest<int>
{
    public string ImportSourceType { get; init; } = string.Empty;

    public Guid PortfolioId { get; init; }

    public Stream DataStream { get; init; } = null!;
}

public sealed class ImportTransactionsFromFileCommandHandler : IRequestHandler<ImportTransactionsFromFileCommand, int>
{
    private readonly ITransactionsImporter m_transactionsImporter;

    public ImportTransactionsFromFileCommandHandler(ITransactionsImporter transactionsImporter)
    {
        m_transactionsImporter = transactionsImporter;
    }

    public async Task<int> Handle(
        ImportTransactionsFromFileCommand request, 
        CancellationToken cancellationToken
        )
    {
        return await m_transactionsImporter.ImportAsync(request.PortfolioId, request.ImportSourceType, request.DataStream);
    }
}