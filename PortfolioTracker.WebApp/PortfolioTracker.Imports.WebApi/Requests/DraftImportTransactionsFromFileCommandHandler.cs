using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;
using PortfolioTracker.Imports.WebApi.Services;

namespace PortfolioTracker.Imports.WebApi.Requests;

public static class DraftImportTransactionsFromFileEndpointExtension
{
    public static void MapDraftImportTransactionsFromFile(this IEndpointRouteBuilder app)
    {
        app.MapPost(@"/draft-import-transactions", DraftImportTransactionsFromFile)
            .WithName(nameof(DraftImportTransactionsFromFile))
            .DisableAntiforgery();
    }

    private static async Task<IResult> DraftImportTransactionsFromFile(
        [FromForm] DraftImportTransactionsFromFileCommand command,
        IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result != null ? TypedResults.Ok(result) : TypedResults.BadRequest();
    }
}

public sealed class DraftImportTransactionsFromFileCommand : IRequest<IEnumerable<BankAccountTransactionGroupModel>>
{
    public Guid BankAccountId { get; init; } 

    public IFormFile File { get; init; } = null!;
}

public sealed class DraftImportTransactionsFromFileCommandHandler : IRequestHandler<DraftImportTransactionsFromFileCommand, IEnumerable<BankAccountTransactionGroupModel>>
{
    private readonly ITransactionsImporter m_transactionsImporter;
    private readonly IPortfolioContext m_context;

    public DraftImportTransactionsFromFileCommandHandler(
        ITransactionsImporter transactionsImporter,
        IPortfolioContext context)
    {
        m_transactionsImporter = transactionsImporter;
        m_context = context;
    }

    public async Task<IEnumerable<BankAccountTransactionGroupModel>> Handle(
        DraftImportTransactionsFromFileCommand request,
        CancellationToken cancellationToken)
    {
        // Get the bank account and its portfolios
        var bankAccount = await m_context.BankAccounts
            //.Include(ba => ba.)
            .FirstOrDefaultAsync(ba => ba.Id == request.BankAccountId, cancellationToken);

        if (bankAccount == null)
        {
            return Enumerable.Empty<BankAccountTransactionGroupModel>();
        }

        // Process the file
        using var stream = request.File.OpenReadStream();
        return await m_transactionsImporter.ImportPreviewAsync(bankAccount.Id, ImportSourceTypes.ScalableCapitalCvs, stream);
    }
}
