import { Card } from "primereact/card";
import { DataTable } from "primereact/datatable";
import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../Store/RootState";
import { Column } from "primereact/column";
import {
  fetchTransactions,
  ITransactionItem,
  setPageIndex,
} from "../Store/TransactionSlice";
import { Paginator, PaginatorPageChangeEvent } from "primereact/paginator";

export default function Transactions() {
  const data = useAppSelector((x) => x.transactions.displayList);
  const loading = useAppSelector((x) => x.transactions.loading);
  const pageSize = useAppSelector((x) => x.transactions.pageSize);
  const recordCount = useAppSelector((x) => x.transactions.recordCount);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fetchTransactions());
  }, []);

  const onPageChange = ($event: PaginatorPageChangeEvent) => {
    dispatch(setPageIndex($event.page));
  };
  const formatDate = (value: Date) => {
    return value.toLocaleDateString("en-US", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    });
  };
  const dateBodyTemplate = (rowData: ITransactionItem) => {
    return formatDate(new Date(rowData.executeDate));
  };

  const inOutTemplate = (rowData: ITransactionItem) => {
    const { inOut } = rowData;

    if (inOut === 1) {
      return "IN";
    } else if (inOut === 2) {
      return "OUT";
    }

    return "";
  };

  const transactionTypeTemplate = (item: ITransactionItem) => {
    const { transactionType } = item;
    if (transactionType === 1) {
      return "Investment";
    } else if (transactionType === 2) {
      return "Expenses";
    } else if (transactionType === 3) {
      return "Dividend";
    }

    return "";
  };

  return (
    <div className="flex pt-16">
      <div className="w-1/4">&nbsp;</div>

      <div className="flex flex-col w-1/2">
        <h2 className="text-green py-8 text-3xl w-full">Transactions</h2>
        <Card className="w-full">
          {loading && <h2>Loading data...</h2>}
          {!loading && (
            <>
              <DataTable value={data}>
                <Column field="stockSymbol" header="Stock" />
                <Column field="price" header="Price" />
                <Column field="quantity" header="Quantity" />
                <Column field="total" header="Total" />
                <Column
                  field="executeDate"
                  header="Execute Date"
                  dataType="date"
                  body={dateBodyTemplate}
                />
                <Column
                  field="transactionType"
                  header="Type"
                  body={transactionTypeTemplate}
                />
                <Column field="inOut" header="In-Out" body={inOutTemplate} />
                <Column field="currencyCode" header="Currency" />
              </DataTable>
              <Paginator
                first={1}
                rows={pageSize}
                totalRecords={recordCount}
                onPageChange={onPageChange}
              />
            </>
          )}
        </Card>
      </div>
      <div className="w-1/4">&nbsp;</div>
    </div>
  );
}
