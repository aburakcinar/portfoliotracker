using MediatR;
using PortfolioTracker.WebApp.Services.TransactionsImporter;

namespace PortfolioTracker.WebApp.Business.Commands.BankTransactionEntity;

public sealed class ImportTransactionsFromFileCommand : IRequest<List<TransactionImportItem>>
{
    public string ImportSourceType { get; init; } = string.Empty;
    
    public Guid PortfolioId { get; init; }

    public Stream DataStream { get; init; } = null!;
}

public sealed class ImportTransactionsFromFileCommandHandler : IRequestHandler<ImportTransactionsFromFileCommand, List<TransactionImportItem>>
{
    private readonly ITransactionsImporter m_transactionsImporter;

    public ImportTransactionsFromFileCommandHandler(ITransactionsImporter transactionsImporter)
    {
        m_transactionsImporter = transactionsImporter;
    }

    public async Task<List<TransactionImportItem>> Handle(ImportTransactionsFromFileCommand request, CancellationToken cancellationToken)
    {
        var items = await m_transactionsImporter.ImportAsync(request.ImportSourceType, request.DataStream);

        await m_transactionsImporter.SaveAsync(request.PortfolioId, items);

        return items;
    }
}