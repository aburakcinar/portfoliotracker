using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Services.TransactionsImporter;

public interface ITransactionImportExtension
{
    string ImportSourceType { get; }

    Task<List<TransactionImportItem>> ImportAsync(Stream dataStream);
}

public static class ImportSourceTypes
{
    public const string ScalableCapitalCvs = @"ScalableCapitalCvs";
}

public interface ITransactionsImporter
{
    Task<List<TransactionImportItem>> ImportAsync(string importSourceType, Stream dataStream);

    Task<bool> SaveAsync(Guid portfolioId, List<TransactionImportItem> transactionImportItems);
}

internal sealed class TransactionsImporter : ITransactionsImporter
{
    private readonly IPortfolioContext m_context;
    private readonly IEnumerable<ITransactionImportExtension> m_extensions;

    public TransactionsImporter(
        IEnumerable<ITransactionImportExtension> extensions,
        IPortfolioContext context
    )
    {
        m_extensions = extensions;
        m_context = context;
    }

    public async Task<List<TransactionImportItem>> ImportAsync(string importSourceType, Stream dataStream)
    {
        var importer = m_extensions.FirstOrDefault(x => x.ImportSourceType == importSourceType);

        if (importer != null)
        {
            return await importer.ImportAsync(dataStream);
        }

        return Enumerable.Empty<TransactionImportItem>().ToList();
    }

    public async Task<bool> SaveAsync(Guid portfolioId, List<TransactionImportItem> transactionImportItems)
    {
        var portfolio = m_context
            .Portfolios
            .Include(x => x.BankAccount)
            .FirstOrDefault(x => x.Id == portfolioId);

        if (portfolio is not { BankAccount: not null })
        {
            return false;
        }

        foreach (var item in transactionImportItems)
        {
            var payload = new SaveImportPayload(portfolioId, portfolio.BankAccountId, item);

            switch (item.TransactionType)
            {
                case nameof(TransactionActionTypes.DEPOSIT):
                    ImportDepositRecord(payload, m_context);
                    break;

                case nameof(TransactionActionTypes.WITHDRAW):
                    ImportWithdrawRecord(payload, m_context);
                    break;

                case nameof(TransactionActionTypes.DIVIDEND_DISTRIBUTION):
                    ImportDistributionRecord(payload, m_context);
                    break;

                case nameof(TransactionActionTypes.ACCOUNT_FEE):
                    ImportAccountFeeRecord(payload, m_context);
                    break;

                case nameof(TransactionActionTypes.INTEREST):
                    ImportInterestRecord(payload,m_context);
                    break;

                case nameof(TransactionActionTypes.BUY_ASSET):
                    ImportBuyAssetRecord(payload, m_context);
                    break;

                case nameof(TransactionActionTypes.SELL_ASSET):
                    ImportSellAssetRecord(payload, m_context);
                    break;
            }
        }

        var count = await m_context.SaveChangesAsync();

        return count > 0;
    }

    private record SaveImportPayload(Guid PortfolioId, Guid BankAccountId, TransactionImportItem Item);

    private void ImportDepositRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;

        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description : @$"Deposit amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };

        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description : @$"Deposit amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.DEPOSIT,
            Quantity = 1m,
            Price = Math.Abs(item.Amount),
            InOut = InOut.Incoming,
        });

        if (item.Fee > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Deposit fee {item.Fee}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.DEPOSIT_FEE,
                Quantity = 1m,
                Price = Math.Abs(item.Fee),
                InOut = InOut.Outgoing,
            });
        }

        if (item.Tax > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Deposit tax {item.Tax}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.DEPOSIT_TAX,
                Quantity = 1m,
                Price = Math.Abs(item.Tax),
                InOut = InOut.Outgoing,
            });
        }

        context.BankAccountTransactionGroups.Add(transactionGroup);
    }

    private void ImportWithdrawRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;

        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Withdraw amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };

        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Withdraw amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.WITHDRAW,
            Quantity = 1m,
            Price = Math.Abs(item.Amount),
            InOut = InOut.Outgoing,
        });

        if (item.Fee > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Withdraw fee {item.Fee}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.WITHDRAW_FEE,
                Quantity = 1m,
                Price = Math.Abs(item.Fee),
                InOut = InOut.Outgoing,
            });
        }

        if (item.Tax > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Withdraw tax {item.Tax}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.WITHDRAW_TAX,
                Quantity = 1m,
                Price = Math.Abs(item.Tax),
                InOut = InOut.Outgoing,
            });
        }

        context.BankAccountTransactionGroups.Add(transactionGroup);
    }

    private void ImportDistributionRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;

        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Dividend amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };

        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Dividend amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.DIVIDEND_DISTRIBUTION,
            Quantity = 1m,
            Price = Math.Abs(item.Amount),
            InOut = InOut.Incoming,
        });

        if (item.Fee > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Dividend distribution fee {item.Fee}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.DIVIDEND_DISTRIBUTION_FEE,
                Quantity = 1m,
                Price = Math.Abs(item.Fee),
                InOut = InOut.Outgoing,
            });
        }

        if (item.Tax > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Deposit tax {item.Tax}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode =
                    TransactionActionTypes.DIVIDEND_WITHHOLDING_TAX, // TODO : Withholding tax means another thing.
                Quantity = 1m,
                Price = Math.Abs(item.Tax),
                InOut = InOut.Outgoing,
            });
        }

        context.BankAccountTransactionGroups.Add(transactionGroup);
    }

    private void ImportAccountFeeRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;

        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Account fee {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };

        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Account fee {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.ACCOUNT_FEE,
            Quantity = 1m,
            Price = Math.Abs(item.Amount),
            InOut = InOut.Outgoing,
        });

        context.BankAccountTransactionGroups.Add(transactionGroup);
    }
    
    private void ImportInterestRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;

        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Dividend amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };

        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Interest amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.INTEREST,
            Quantity = 1m,
            Price = Math.Abs(item.Amount),
            InOut = InOut.Incoming,
        });

        if (item.Fee > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Interest fee {item.Fee}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.INTEREST_FEE,
                Quantity = 1m,
                Price = Math.Abs(item.Fee),
                InOut = InOut.Outgoing,
            });
        }

        if (item.Tax > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Interest tax {item.Tax}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode =
                    TransactionActionTypes.INTEREST_TAX,
                Quantity = 1m,
                Price = Math.Abs(item.Tax),
                InOut = InOut.Outgoing,
            });
        }

        context.BankAccountTransactionGroups.Add(transactionGroup);
    }

    private void ImportBuyAssetRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;
        var asset = item.Asset;
        var portfolio = context.Portfolios.FirstOrDefault(p => p.Id == payload.PortfolioId);

        if (asset == null || portfolio == null)
        {
            return;
        }
        
        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Buy asset amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };
        
        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Buy asset amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.BUY_ASSET,
            Quantity = item.Quantity,
            Price = Math.Abs(item.Price),
            InOut = InOut.Outgoing,
        });

        if (item.Fee > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Buy asset fee {item.Fee}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.BUY_ASSET_FEE,
                Quantity = 1m,
                Price = Math.Abs(item.Fee),
                InOut = InOut.Outgoing,
            });
        }

        if (item.Tax > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Buy asset tax {item.Tax}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode =
                    TransactionActionTypes.BUY_ASSET_TAX,
                Quantity = 1m,
                Price = Math.Abs(item.Tax),
                InOut = InOut.Outgoing,
            });
        }

        var holding = new Holding
        {
            Id = Guid.NewGuid(),
            Asset = asset,
            Portfolio = portfolio,
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            PortfolioId = portfolio.Id,
            AssetId = asset.Id,
            BankAccountTransactionGroup = transactionGroup,
            BankAccountTransactionGroupId = transactionGroup.Id,
        };
        
        context.Holdings.Add(holding);
    }
    
    private void ImportSellAssetRecord(
        SaveImportPayload payload,
        IPortfolioContext context
    )
    {
        var bankAccountId = payload.BankAccountId;
        var item = payload.Item;
        var asset = item.Asset;
        var portfolio = context.Portfolios.FirstOrDefault(p => p.Id == payload.PortfolioId);

        if (asset == null || portfolio == null)
        {
            return;
        }
        
        var transactionGroup = new BankAccountTransactionGroup
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Sell asset amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            State = TransactionGroupState.Executed,
            ReferenceNo = item.ReferenceNo,
        };
        
        transactionGroup.Transactions.Add(new BankAccountTransaction
        {
            Id = Guid.NewGuid(),
            Description = !string.IsNullOrEmpty(item.Description) ? item.Description :  @$"Sell asset amount {item.Amount}",
            Created = DateTime.Now.ToUniversalTime(),
            ActionTypeCode = TransactionActionTypes.SELL_ASSET,
            Quantity = item.Quantity,
            Price = Math.Abs(item.Price),
            InOut = InOut.Incoming,
        });

        if (item.Fee > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Sell asset fee {item.Fee}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode = TransactionActionTypes.SELL_ASSET_FEE,
                Quantity = 1m,
                Price = Math.Abs(item.Fee),
                InOut = InOut.Outgoing,
            });
        }

        if (item.Tax > 0)
        {
            transactionGroup.Transactions.Add(new BankAccountTransaction
            {
                Id = Guid.NewGuid(),
                Description = @$"Sell asset tax {item.Tax}",
                Created = DateTime.Now.ToUniversalTime(),
                ActionTypeCode =
                    TransactionActionTypes.SELL_ASSET_TAX,
                Quantity = 1m,
                Price = Math.Abs(item.Tax),
                InOut = InOut.Outgoing,
            });
        }

        var holding = new Holding
        {
            Id = Guid.NewGuid(),
            Asset = asset,
            Portfolio = portfolio,
            Created = DateTime.Now.ToUniversalTime(),
            ExecuteDate = DateOnly.FromDateTime(item.ExecuteDate),
            PortfolioId = portfolio.Id,
            AssetId = asset.Id,
            BankAccountTransactionGroup = transactionGroup,
            BankAccountTransactionGroupId = transactionGroup.Id,
        };
        
        context.Holdings.Add(holding);
    }
}