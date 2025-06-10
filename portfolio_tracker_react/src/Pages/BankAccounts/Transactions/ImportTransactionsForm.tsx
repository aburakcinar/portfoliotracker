import React, { useState, useRef } from "react";
import { useParams } from "react-router";
import { useBankAccount } from "../../../Hooks";
import { draftImportTransactionsFromFileApi, ImportTransactionsFromFileCommand } from "../../../Api/Transaction.api";
//import { DataTable } from "primereact/datatable";
//import { Column } from "primereact/column";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import { ProgressSpinner } from "primereact/progressspinner";
import { Timeline } from "primereact/timeline";
import { Card } from "primereact/card";
import { Tag } from "primereact/tag";
import { IBankAccountTransactionGroupModel, IBankAccountTransactionModel, InOut, TransactionType } from "../../../Api";

export const ImportTransactionsForm: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const bankAccount = useBankAccount(id);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [fileName, setFileName] = useState<string>("");
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isLoaded, setIsLoaded] = useState<boolean>(true);
    interface GroupedTransactions {
        date: string;
        transactions: IBankAccountTransactionGroupModel[];
    }

    const groupByExecuteDate = (transactions: IBankAccountTransactionGroupModel[]): GroupedTransactions[] => {
        const groups: { [date: string]: IBankAccountTransactionGroupModel[] } = {};
        transactions.forEach((tx) => {
            const date = new Date(tx.executeDate).toISOString().split('T')[0]; // YYYY-MM-DD
            if (!groups[date]) groups[date] = [];
            groups[date].push(tx);
        });
        return Object.entries(groups)
            .sort(([a], [b]) => new Date(b).getTime() - new Date(a).getTime()) // latest date first
            .map(([date, transactions]) => ({ date, transactions }));
    };

    const [groupedDraftTransactions, setGroupedDraftTransactions] = useState<GroupedTransactions[]>(() => {
        const fakeData = require('./FakeData.json') as IBankAccountTransactionGroupModel[];
        return groupByExecuteDate(fakeData);
    });

    // Keep for compatibility, but unused after refactor
    const [draftTransactions, setDraftTransactions] = useState<IBankAccountTransactionGroupModel[]>([]);
    const fileInputRef = useRef<HTMLInputElement>(null);
    const toast = useRef<Toast>(null);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const files = event.target.files;
        if (files && files.length > 0) {
            setSelectedFile(files[0]);
            setFileName(files[0].name);
        }
    };

    const handleFileUpload = async () => {
        if (!selectedFile || !id) {
            toast.current?.show({
                severity: "error",
                summary: "Error",
                detail: "Please select a file to upload",
                life: 3000
            });
            return;
        }

        setIsLoading(true);

        try {
            const command: ImportTransactionsFromFileCommand = {
                bankAccountId: id,
                file: selectedFile
            };

            const result = await draftImportTransactionsFromFileApi(command);
            setDraftTransactions(result); // keep for compatibility
            setGroupedDraftTransactions(groupByExecuteDate(result));
            setIsLoaded(true);
            toast.current?.show({
                severity: "success",
                summary: "Success",
                detail: `${result.length} transactions imported successfully`,
                life: 3000
            });
        } catch (error) {
            console.error("Error importing transactions:", error);
            toast.current?.show({
                severity: "error",
                summary: "Error",
                detail: "Failed to import transactions. Please try again.",
                life: 3000
            });
            setIsLoaded(false);
        } finally {
            setIsLoading(false);
        }
    };

    const resetForm = () => {
        setSelectedFile(null);
        setFileName("");
        setIsLoaded(false);
        setDraftTransactions([]);
        setGroupedDraftTransactions([]);
        if (fileInputRef.current) {
            fileInputRef.current.value = "";
        }
    };

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat(bankAccount?.localeCode || "en-US", {
            style: "currency",
            currency: bankAccount?.currencyCode || "USD"
        }).format(value);
    };

    const formatDate = (value: Date) => {
        return new Date(value).toLocaleDateString("en-GB");
    };


    // Get transaction type label and severity
    const getTransactionTypeInfo = (transactionType: TransactionType) => {
        const types: Record<number, { label: string, severity: 'success' | 'info' | 'warning' | 'danger' }> = {
            [TransactionType.Main]: { label: 'Main', severity: 'info' },
            [TransactionType.Tax]: { label: 'Tax', severity: 'warning' },
            [TransactionType.Fee]: { label: 'Fee', severity: 'danger' }
        };

        return types[transactionType] || { label: 'Unknown', severity: 'info' };
    };

    // const priceBodyTemplate = (rowData: IDraftTransactionItem) => {
    //     return formatCurrency(rowData.price);
    // };

    // const totalBodyTemplate = (rowData: IDraftTransactionItem) => {
    //     return formatCurrency(rowData.total);
    // };

    // const dateBodyTemplate = (rowData: IDraftTransactionItem) => {
    //     return formatDate(rowData.executeDate);
    // };

    const renderItems = (items: IBankAccountTransactionGroupModel[]) => {

        return items.map((item: IBankAccountTransactionGroupModel) => renderItem(item));
    };

    const getTransactionType = (item: IBankAccountTransactionModel) => {
        switch (item.transactionType) {

            case TransactionType.Tax:
                return "Tax :";
            case TransactionType.Fee:
                return "Fee :";
            default:
                return "";
        }
    }

    const renderItem = (item: IBankAccountTransactionGroupModel) => {

        return <Card
            title={item.actionTypeName}
            className="mb-3"
        >
            <div className="p-2">
                {item.transactions.map((transaction: IBankAccountTransactionModel) => (
                    <div key={transaction.id} className="flex justify-content-between">
                        <span>{getTransactionType(transaction)} {formatCurrency(transaction.quantity * transaction.price)}</span>
                    </div>
                ))}
            </div>
        </Card>
    };

    return (
        <div className="flex flex-col w-full px-5">
            <Toast ref={toast} />
            <h2 className="text-green pb-4 text-5xl">Transactions - Import</h2>

            {bankAccount && (
                <div className="flex flex-row">
                    <h3 className="grow-0 text-green pb-8 text-2xl">{bankAccount.name}</h3>
                    <div className="grow"></div>
                    <h3 className="grow-0 text-gray-500 pb-8 text-xl">{bankAccount.iban}</h3>
                </div>
            )}

            {!isLoaded && <div className="mb-6">
                <h3 className="text-xl mb-2">File Selection</h3>
                <div className="flex flex-col">
                    <label htmlFor="transaction-file" className="mb-2">Select a transaction file (.csv, .xlsx, .xls)</label>
                    <div className="flex items-center">
                        <input
                            id="transaction-file"
                            type="file"
                            ref={fileInputRef}
                            onChange={handleFileChange}
                            accept=".csv,.xlsx,.xls"
                            className="mr-2 p-2 border rounded"
                            aria-label="Transaction file"
                        />
                        <Button
                            label="Upload"
                            onClick={handleFileUpload}
                            disabled={!selectedFile || isLoading}
                            className="mr-2"
                        />
                        <Button
                            label="Reset"
                            onClick={resetForm}
                            className="p-button-secondary"
                            disabled={isLoading}
                        />
                    </div>
                </div>
            </div>
            }
            {fileName && <p className="mt-2">Selected file: {fileName}</p>}

            {isLoading && (
                <div className="flex justify-center items-center my-4">
                    <ProgressSpinner style={{ width: '50px', height: '50px' }} />
                    <span className="ml-2">Processing file...</span>
                </div>
            )}


            <Timeline
                value={groupedDraftTransactions}
                align="left"
                className="w-full"
                marker={(item) => (
                    <span className="flex size-2 align-items-center justify-content-center text-white border-circle z-1 shadow-1">
                        <i className="pi pi-calendar"></i>
                    </span>
                )}
                opposite={(item) => (
                    <div className="flex flex-col-reverse font-semibold text-right">
                        {formatDate(item.date)}
                    </div>
                )}
                content={(item: GroupedTransactions) => renderItems(item.transactions)}
            />
        </div>
    );
}
