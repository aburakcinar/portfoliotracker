using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;

namespace PortfolioTracker.Imports.WebApi.Services.Handlers;

/// <summary>
/// Handler for buy asset transactions
/// </summary>
public class BuyAssetTransactionHandler : BaseTransactionTypeHandler
{
    /// <inheritdoc />
    public override string TransactionType => nameof(TransactionActionTypes.BUY_ASSET);

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
            Quantity = Math.Abs(item.Quantity),
            Price = Math.Abs(item.Price),
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
            Description = $"Buy asset fee {item.Fee}",
            Quantity = 1m,
            Price = Math.Abs(item.Fee),
            InOut = InOut.Outgoing,
            TransactionType = Data.Models.TransactionType.Fee
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
            Description = $"Buy asset tax {item.Tax}",
            Quantity = 1m,
            Price = Math.Abs(item.Tax),
            InOut = InOut.Outgoing,
            TransactionType = Data.Models.TransactionType.Tax
        });
    }

    /// <inheritdoc />
    protected override string GetDefaultDescription(TransactionImportItem item)
    {
        return !string.IsNullOrEmpty(item.Description) ? item.Description : $"Buy asset amount {item.Amount}";
    }

    /// <inheritdoc />
    protected override void ProcessAdditionalOperations(Guid portfolioId, BankAccountTransactionGroup transactionGroup, TransactionImportItem item, IPortfolioContext context)
    {
        //var asset = item.Asset;
        //var portfolio = context.Portfolios.FirstOrDefault(p => p.Id == portfolioId);

        //if (asset == null || portfolio == null)
        //{
        //    return;
        //}

        //var holding = new Holding
        //{
        //    Id = Guid.NewGuid(),
        //    Asset = asset,
        //    Portfolio = portfolio,
        //    Created = DateTime.Now.ToUniversalTime(),
        //    ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
        //    PortfolioId = portfolio.Id,
        //    AssetId = asset.Id,
        //    BankAccountTransactionGroup = transactionGroup,
        //    BankAccountTransactionGroupId = transactionGroup.Id,
        //};

        //context.Holdings.Add(holding);
    }
}
