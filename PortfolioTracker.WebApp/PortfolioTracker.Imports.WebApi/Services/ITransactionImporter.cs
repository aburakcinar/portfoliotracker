using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;
using PortfolioTracker.Imports.WebApi.Services.Handlers;
using System.Collections.Generic;

namespace PortfolioTracker.Imports.WebApi.Services;

/// <summary>
///     Defines the interface for a transaction importer.
/// </summary>
public interface ITransactionsImporter
{
    /// <summary>
    ///     Imports transactions from a data stream based on the specified import source type.
    /// </summary>
    /// <param name="bankAccountId"></param>
    /// <param name="importSourceType"></param>
    /// <param name="dataStream"></param>
    /// <returns>Returns number of record</returns>
    Task<int> ImportAsync(Guid bankAccountId, string importSourceType, Stream dataStream);

    /// <summary>
    ///     Imports transactions as preview.
    /// </summary>
    /// <param name="bankAccountId"></param>
    /// <param name="importSourceType"></param>
    /// <param name="dataStream"></param>
    /// <returns>List of transaction preview</returns>
    Task<IEnumerable<BankAccountTransactionGroupModel>> ImportPreviewAsync(Guid bankAccountId, string importSourceType, Stream dataStream);
}

public interface ITransactionImportExtension
{
    string ImportSourceType { get; }

    Task<List<TransactionImportItem>> ImportAsync(Stream dataStream);
}

public static class ImportSourceTypes
{
    public const string ScalableCapitalCvs = @"ScalableCapitalCvs";
}

internal sealed class TransactionsImporter : ITransactionsImporter
{
    private readonly IPortfolioContext m_context;
    private readonly IEnumerable<ITransactionImportExtension> m_extensions;
    private readonly IEnumerable<ITransactionTypeHandler> m_transactionTypeHandlers;

    public TransactionsImporter(
        IPortfolioContext context,
        IEnumerable<ITransactionImportExtension> extensions,
        IEnumerable<ITransactionTypeHandler> transactionTypeHandlers
        )
    {
        m_context = context;
        m_extensions = extensions;
        m_transactionTypeHandlers = transactionTypeHandlers;
    }

    public async Task<int> ImportAsync(Guid portfolioId, string importSourceType, Stream dataStream)
    {
        var importer = m_extensions.FirstOrDefault(x => x.ImportSourceType == importSourceType);

        if (importer != null)
        {
            var data = await importer.ImportAsync(dataStream);

            await SaveAsync(portfolioId, data);
        }

        return 0;
    }

    public async Task<IEnumerable<BankAccountTransactionGroupModel>> ImportPreviewAsync(Guid bankAccountId, string importSourceType, Stream dataStream)
    {
        var importer = m_extensions.FirstOrDefault(x => x.ImportSourceType == importSourceType);

        if (importer != null)
        {
            var data = await importer.ImportAsync(dataStream);

            //await SaveAsync(portfolioId, data);

            var bankAccount = m_context
                .BankAccounts
                .FirstOrDefault(x => x.Id == bankAccountId);

            var transactionActionTypes = await m_context.TransactionActionTypes.ToListAsync();

            var lstResult = new List<BankAccountTransactionGroupModel>();

            foreach (var item in data)
            {
                var handler = m_transactionTypeHandlers.FirstOrDefault(h => h.TransactionType == item.TransactionType);

                // Generate the transaction group model using the handler
                var transactionGroup = handler?.GetTransaction(bankAccountId, item, m_context);
                
                if (transactionGroup != null)
                {
                    transactionGroup.ActionTypeName = transactionActionTypes.FirstOrDefault(x => x.Code == item.TransactionType)?.Name ?? string.Empty;

                    lstResult.Add(transactionGroup);
                }
            }

            return lstResult;
        }

        return Enumerable.Empty<BankAccountTransactionGroupModel>();
    }

    public async Task<bool> SaveAsync(Guid bankAccountId, List<TransactionImportItem> transactionImportItems)
    {
        var bankAccount = m_context
            .BankAccounts
            .FirstOrDefault(x => x.Id == bankAccountId);

        if (bankAccount is null)
        {
            return false;
        }

        foreach (var item in transactionImportItems)
        {
            // Find the appropriate handler for this transaction type
            var handler = m_transactionTypeHandlers.FirstOrDefault(h => h.TransactionType == item.TransactionType);

            if (handler != null)
            {
                // Let the handler process the import
                handler.ProcessImport(bankAccount.Id, item, m_context);
            }
            else
            {
                // Log or handle unsupported transaction types
                Console.WriteLine($"No handler found for transaction type: {item.TransactionType}");
            }
        }

        var count = await m_context.SaveChangesAsync();

        return count > 0;
    }
}