using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;

namespace PortfolioTracker.Imports.WebApi.Services.Handlers;

/// <summary>
/// Handler for account fee transactions
/// </summary>
public class AccountFeeTransactionHandler : BaseTransactionTypeHandler
{
    /// <inheritdoc />
    public override string TransactionType => nameof(TransactionActionTypes.ACCOUNT_FEE);

    /// <inheritdoc />
    protected override void AddMainTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item)
    {
        var transaction = transactionGroup.Transactions.FirstOrDefault(x => x.TransactionType == Data.Models.TransactionType.Main);

        if (transaction is not null)
        {
            return;
        }

        transactionGroup.Transactions.Add(new BankAccountTransactionModel
        {
            Id = Guid.NewGuid(),
            Description = GetDefaultDescription(item),
            Quantity = 1m,
            Price = Math.Abs(item.Amount),
            InOut = InOut.Outgoing,
            TransactionType = Data.Models.TransactionType.Main
        });
    }

    /// <inheritdoc />
    protected override void AddFeeTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item)
    {
        // Account fee transaction doesn't have a separate fee transaction
    }

    /// <inheritdoc />
    protected override void AddTaxTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item)
    {
        // Account fee transaction doesn't have a separate tax transaction
    }

    /// <inheritdoc />
    protected override string GetDefaultDescription(TransactionImportItem item)
    {
        return !string.IsNullOrEmpty(item.Description) ? item.Description : $"Account fee {item.Amount}";
    }
}
