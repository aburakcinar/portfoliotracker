import React from "react";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { ITransactionItem } from "../../../Store/Transaction.slice";
import classNames from "classnames";

interface TransactionListFormProps {
    transactions: ITransactionItem[];
}

export const TransactionListForm: React.FC<TransactionListFormProps> = ({ transactions }) => {
    const amountItemTemplate = (item: ITransactionItem) => {
        const divClassNames = classNames(
            { "text-emerald-500": item.price >= 0 },
            { "text-red-500": item.price < 0 }
        );

        const amount = item.price * item.quantity;

        return <span className={divClassNames}>{amount.toFixed(2)}</span>;
    };

    return (
       
            <DataTable value={transactions} className="" size="small">
                <Column
                    field="operationDate"
                    header="Date"
                    className="w-24"
                />
                <Column field="description" />
                <Column
                    field="amount"
                    header="Amount"
                    body={amountItemTemplate}
                    className="text-right w-24"
                />
            </DataTable>
    );
};
