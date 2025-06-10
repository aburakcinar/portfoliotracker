using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Extensions;
using PortfolioTracker.Imports.WebApi.Models;

namespace PortfolioTracker.Imports.WebApi.Services;

/// <summary>
/// Base abstract implementation for transaction type handlers
/// </summary>
public abstract class BaseTransactionTypeHandler : ITransactionTypeHandler
{
    /// <summary>
    /// Gets the transaction action type that this handler can process
    /// </summary>
    public abstract string TransactionType { get; }

    /// <summary>
    /// Process the import of a specific transaction type
    /// </summary>
    public virtual void ProcessImport(
        Guid bankAccountId, 
        TransactionImportItem item, 
        IPortfolioContext context
        )
    {
        var transactionGroup = CreateTransactionGroup(bankAccountId, item);

        // Add the main transaction
        AddMainTransaction(transactionGroup, item);

        // Add fee transaction if needed
        if (item.Fee > 0)
        {
            AddFeeTransaction(transactionGroup, item);
        }

        // Add tax transaction if needed
        if (item.Tax > 0)
        {
            AddTaxTransaction(transactionGroup, item);
        }

        // Process additional operations (like adding holdings for asset transactions)
        //ProcessAdditionalOperations(portfolioId, transactionGroup, item, context);

        // Add the transaction group to the context
        //context.BankAccountTransactionGroups.Add(transactionGroup);
    }

    public BankAccountTransactionGroupModel GetTransaction(
            Guid bankAccountId,
            TransactionImportItem item,
            IPortfolioContext context
        )
    {
        var bankAccountTransactionGroupItem = context
            .BankAccountTransactionGroups
            .Include(x => x.Transactions)
            .FirstOrDefault(x => x.ReferenceNo == item.ReferenceNo);

        if(bankAccountTransactionGroupItem is not null)
        {
            return bankAccountTransactionGroupItem.ToModel();
        }

        var transactionActionType = context.TransactionActionTypes.FirstOrDefault(x => x.Code == item.TransactionType);

        var bankAccountTransactionGroupModel = new BankAccountTransactionGroupModel
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            ReferenceNo = item.ReferenceNo,
            Description = item.Description,
            ExecuteDate = item.ExecuteDate,
            Created = DateTime.Now.ToUniversalTime(),
            IsDraft = true,
            ActionTypeCode = transactionActionType?.Code ?? string.Empty,
            ActionTypeName = transactionActionType?.Name ?? string.Empty,            
        };

        if (bankAccountTransactionGroupItem is not null)
        {
            bankAccountTransactionGroupModel.Id = bankAccountTransactionGroupItem.Id;
            bankAccountTransactionGroupModel.IsDraft = false;
            bankAccountTransactionGroupModel.Transactions = bankAccountTransactionGroupItem
                .Transactions
                .Select(x => x.ToModel())
                .ToList();
        }

        AddMainTransaction(bankAccountTransactionGroupModel, item);

        // Add fee transaction if needed
        if (item.Fee > 0)
        {
            AddFeeTransaction(bankAccountTransactionGroupModel, item);
        }

        // Add tax transaction if needed
        if (item.Tax > 0)
        {
            AddTaxTransaction(bankAccountTransactionGroupModel, item);
        }

        return bankAccountTransactionGroupModel;
    }

    /// <summary>
    /// Creates a new transaction group for the import
    /// </summary>
    protected virtual BankAccountTransactionGroupModel CreateTransactionGroup(Guid bankAccountId, TransactionImportItem item)
    {
        return new BankAccountTransactionGroupModel
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description : GetDefaultDescription(item),
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = item.ExecuteDate,
            ReferenceNo = item.ReferenceNo,
            IsDraft = true,            
        };
    }

    /// <summary>
    /// Adds the main transaction to the transaction group
    /// </summary>
    protected abstract void AddMainTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item);

    /// <summary>
    /// Adds a fee transaction to the transaction group if applicable
    /// </summary>
    protected abstract void AddFeeTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item);

    /// <summary>
    /// Adds a tax transaction to the transaction group if applicable
    /// </summary>
    protected abstract void AddTaxTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item);

    /// <summary>
    /// Gets the default description for the transaction
    /// </summary>
    protected abstract string GetDefaultDescription(TransactionImportItem item);

    /// <summary>
    /// Process any additional operations required for this transaction type
    /// </summary>
    
    protected virtual void ProcessAdditionalOperations(Guid portfolioId, BankAccountTransactionGroup transactionGroup, TransactionImportItem item, IPortfolioContext context)
    {
        // Default implementation does nothing, override in derived classes if needed
    }

}
