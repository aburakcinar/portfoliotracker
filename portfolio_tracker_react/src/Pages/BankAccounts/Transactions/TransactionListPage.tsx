import React, { useEffect } from "react";
import { useParams } from "react-router";
import { listTransactionsByBankAccountIdApi } from "../../../Api";
import { ITransactionItem } from "../../../Store/Transaction.slice";
import { TransactionListForm } from "./TransactionListForm";
import { useBankAccount } from "../../../Hooks";
import { AddTransactionMenu } from "./AddTransactionMenu";
import { AddDepositForm } from "./AddDepositForm";

export const TransactionListPage: React.FC = () => {
    const { id } = useParams();
    const [transactions, setTransactions] = React.useState<ITransactionItem[]>([]);

    const bankAccount = useBankAccount(id);

    useEffect(() => {
        if (id) {
            listTransactionsByBankAccountIdApi(id)
                .then((data) => {
                    setTransactions(data);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }, [id]);

    if (!bankAccount) {
        return null;
    }

    return (
        <div className="flex flex-col w-full px-5">
            <h2 className="text-green pb-8 text-5xl">Transactions</h2>


            <div className="flex flex-row">
                <h3 className="grow-0 text-green pb-8 text-2xl">{bankAccount.name}</h3>
                <div className="grow"></div>
                <h3 className="grow-0 text-gray-500 pb-8 text-xl">{bankAccount.iban}</h3>
            </div>

            <AddTransactionMenu bankAccountId={bankAccount.id} currencyCode={bankAccount.currencyCode} />

            <div className="my-4 border-t border-t-gray-200"></div>

            <div className="grid grid-cols-3 gap-4">
                <div className="col-span-2">
                    <TransactionListForm transactions={transactions} />
                </div>
                <div className="col-span-1">
                    <AddDepositForm bankAccountId={bankAccount.id} currencyCode={bankAccount.currencyCode} />
                </div>
            </div>

        </div>
    );
};