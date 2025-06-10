using PortfolioTracker.Data.Models;
using PortfolioTracker.Imports.WebApi.Models;

namespace PortfolioTracker.Imports.WebApi.Extensions;

public static class BankAccountTransactionGroupExtensions
{
    public static BankAccountTransactionGroup ToEntity(
        this BankAccountTransactionGroupModel model
        )
    {
        return new BankAccountTransactionGroup
        {
            Id = model.Id,
            BankAccountId = model.BankAccountId,
            ReferenceNo = model.ReferenceNo,
            Created = model.Created,
            Description = model.Description,
            ExecuteDate = model.ExecuteDate,
            ActionTypeCode = model.ActionTypeCode,
            Updated = model.Updated,
            Transactions = model.Transactions.Select(t => t.ToEntity()).ToList()
        };
    }

    public static BankAccountTransactionGroupModel ToModel(
        this BankAccountTransactionGroup transactionGroup
        )
    {
        return new BankAccountTransactionGroupModel
        {
            Id = transactionGroup.Id,
            BankAccountId = transactionGroup.BankAccountId,
            ReferenceNo = transactionGroup.ReferenceNo,
            Created = transactionGroup.Created,
            Description = transactionGroup.Description,
            Transactions = transactionGroup.Transactions.Select(t => t.ToModel()).ToList(),
            ActionTypeCode = transactionGroup.ActionTypeCode,
            IsDraft = false,
            ExecuteDate = transactionGroup.ExecuteDate,
            Updated = transactionGroup.Updated
        };
    }
}

public static class BankAccountTransactionExtensions
{
    public static BankAccountTransaction ToEntity(this BankAccountTransactionModel model)
    {
        return new BankAccountTransaction
        {
            Id = model.Id,
            BankAccountTransactionGroupId = model.BankAccountTransactionGroupId,
            Price = model.Price,
            Quantity = model.Quantity,
            InOut = model.InOut,
            Description = model.Description,
        };
    }

    public static BankAccountTransactionModel ToModel(this BankAccountTransaction transaction)
    {
        return new BankAccountTransactionModel
        {
            Id = transaction.Id,
            BankAccountTransactionGroupId = transaction.BankAccountTransactionGroupId,
            Price = transaction.Price,
            Quantity = transaction.Quantity,
            InOut = transaction.InOut,
            Description = transaction.Description,
        };
    }
}
