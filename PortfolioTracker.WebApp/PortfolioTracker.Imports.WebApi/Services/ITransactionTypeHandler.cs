using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;

namespace PortfolioTracker.Imports.WebApi.Services;

/// <summary>
/// Interface for handling specific transaction types during import
/// </summary>
public interface ITransactionTypeHandler
{
    /// <summary>
    /// Gets the transaction action type that this handler can process
    /// </summary>
    string TransactionType { get; }

    /// <summary>
    /// Process the import of a specific transaction type
    /// </summary>
    /// <param name="portfolioId">The portfolio ID</param>
    /// <param name="bankAccountId">The bank account ID</param>
    /// <param name="item">The transaction import item</param>
    /// <param name="context">The portfolio context</param>
    void ProcessImport(Guid bankAccountId, TransactionImportItem item, IPortfolioContext context);

    /// <summary>
    ///     Generates Transaction group item with imported record.
    /// </summary>
    /// <param name="bankAccountId"></param>
    /// <param name="portfolioId"></param>
    /// <param name="item"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    BankAccountTransactionGroupModel GetTransaction(Guid bankAccountId, TransactionImportItem item, IPortfolioContext context);
}
