import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { IBankTransactionGroupModel } from "../../Api/BankTransactions.api";
import { classNames } from "primereact/utils";
import { listBankAccountTransactions } from "../../Store/Transaction.slice";

export interface IListTransactionsFormProps {
  bankAccountId: string;
  currencyCode: string;
  currencySymbol: string;
}

export const ListTransactionsForm: React.FC<IListTransactionsFormProps> = (
  props
) => {
  const { bankAccountId } = props;

  const dispatch = useAppDispatch();
  const transactions = useAppSelector((x) =>
    x.transactions.bankTransactions.filter(
      (p) => p.bankAccountId === bankAccountId
    )
  );

  useEffect(() => {
    dispatch(listBankAccountTransactions(bankAccountId));
  }, []);

  const amountItemTemplate = (item: IBankTransactionGroupModel) => {
    const divClassNames = classNames(
      { "text-emerald-500": item.amount >= 0 },
      { "text-red-500": item.amount < 0 }
    );

    return <span className={divClassNames}>{item.amount.toFixed(2)}</span>;
  };

  const balanceItemTemplate = (item: IBankTransactionGroupModel) => {
    const divClassNames = classNames(
      { "text-emerald-500": item.balance >= 0 },
      { "text-red-500": item.balance < 0 }
    );

    return <span className={divClassNames}>{item.balance.toFixed(2)}</span>;
  };

  return (
    <DataTable value={transactions} className="" size="small">
      <Column
        field="operationDate"
        header="Date"
        body={(item: IBankTransactionGroupModel) =>
          new Date(item.operationDate).toLocaleDateString("tr-TR", {
            day: "numeric",
            month: "numeric",
            year: "numeric",
          })
        }
        className="w-24"
      />
      <Column field="description" />
      <Column
        field="amount"
        header="Amount"
        body={amountItemTemplate}
        className="text-right w-24"
      />
      <Column
        field="balance"
        header="Balance"
        body={balanceItemTemplate}
        className="text-right w-24"
      />
    </DataTable>
  );
};
