using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;

namespace PortfolioTracker.Imports.WebApi.Services.Handlers;

/// <summary>
/// Handler for withdraw transactions
/// </summary>
public class WithdrawTransactionHandler : BaseTransactionTypeHandler
{
    /// <inheritdoc />
    public override string TransactionType => nameof(TransactionActionTypes.WITHDRAW);

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
        var transaction = transactionGroup.Transactions.FirstOrDefault(x => x.TransactionType == Data.Models.TransactionType.Fee);

        if (transaction is not null)
        {
            return;
        }

        transactionGroup.Transactions.Add(new BankAccountTransactionModel
        {
            Id = Guid.NewGuid(),
            Description = $"Withdraw fee {item.Fee}",
            Quantity = 1m,
            Price = Math.Abs(item.Fee),
            InOut = InOut.Outgoing,
            TransactionType = Data.Models.TransactionType.Fee,
        });
    }

    /// <inheritdoc />
    protected override void AddTaxTransaction(BankAccountTransactionGroupModel transactionGroup, TransactionImportItem item)
    {
        var transaction = transactionGroup.Transactions.FirstOrDefault(x => x.TransactionType == Data.Models.TransactionType.Tax);

        if (transaction is not null)
        {
            return;
        }

        transactionGroup.Transactions.Add(new BankAccountTransactionModel
        {
            Id = Guid.NewGuid(),
            Description = $"Withdraw tax {item.Tax}",
            Quantity = 1m,
            Price = Math.Abs(item.Tax),
            InOut = InOut.Outgoing,
            TransactionType= Data.Models.TransactionType.Tax,
        });
    }

    /// <inheritdoc />
    protected override string GetDefaultDescription(TransactionImportItem item)
    {
        return !string.IsNullOrEmpty(item.Description) ? item.Description : $"Withdraw amount {item.Amount}";
    }
}
