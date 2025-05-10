
import React, { ChangeEvent, useRef, useState } from "react";
import { ArrowUpTrayIcon } from "@heroicons/react/24/outline";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { importBankAccountsApi } from "../../Api";
import { useNavigate } from "react-router";

export interface ImportItem {
    name: string;
    bankName: string;
    accountHolder: string;
    description: string;
    iban: string;
    currencyCode: string;
    localeCode: string;
    openDate: Date;
}

export const ImportBankAccountsForm: React.FC = () => {
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const fileInputRef = useRef<HTMLInputElement | null>(null);
    const [jsonData, setJsonData] = useState<ImportItem[]>([]);
    const [dataToImport, setDataToImport] = useState<ImportItem[]>([]);
    const navigate = useNavigate();

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const files = event.target.files;


        if (files && files.length > 0) {

            const file = files.item(0);
            setSelectedFile(file);
            if (file) {
                readFile(file);
            }
        } else {
            setSelectedFile(null);
            setJsonData([]);
        }
    };

    const handleButtonClick = () => {
        if (fileInputRef?.current) {
            fileInputRef.current.click();
        }
    };

    const importSelectedHandler = () => {
        console.log("Importing selected data:", dataToImport);

        importBankAccountsApi(dataToImport)
            .then((resp) => navigate("/bankaccounts"))
            .catch((error) => console.error("Import error:", error));
    };

    const readFile = (file: File) => {
        const reader = new FileReader();

        reader.onload = (event) => {
            try {
                const result = event.target?.result;
                if (typeof result === 'string') {
                    const parsedData: ImportItem[] = JSON.parse(result);
                    setJsonData(parsedData);
                } else {
                    console.error('File content is not a string.');
                    setJsonData([]);
                }
            } catch (error) {
                console.error('Error parsing JSON:', error);
                setJsonData([]);
            }
        };

        reader.onerror = (error) => {
            console.error('Error reading file:', error);
            setJsonData([]);
        };

        reader.readAsText(file);
    };

    return (
        <div className="flex flex-col min-w-[500px] w-1/2 ">
            <h2 className="text-green pb-8 text-5xl">Import Bank Accounts</h2>

            <div className="flex items-center">

                <input
                    type="file"
                    className="hidden"
                    onChange={handleFileChange}
                    ref={fileInputRef}
                />
                <div className="grow">
                    {selectedFile ? (
                        <span className="text-gray-700">{selectedFile.name}</span>
                    ) : (
                        <span className="text-gray-500">No file selected</span>
                    )}
                </div>

                {!selectedFile && <button
                    className="bg-green h-10 flex flex-row p-2 mr-1"
                    onClick={handleButtonClick}>
                    <ArrowUpTrayIcon className="size-5 mr-1" />
                    <span>Select File</span>
                </button>}
                {selectedFile && <button
                    className="bg-green h-10 flex flex-row p-2 mr-1"
                    onClick={importSelectedHandler}>
                    <ArrowUpTrayIcon className="size-5 mr-1" />
                    <span>Import Selected</span>
                </button>}
            </div>

            {jsonData.length > 0 && (
                <div className="overflow-x-auto">
                    <DataTable
                        value={jsonData}
                        selectionMode={'multiple'} selection={dataToImport!}
                        onSelectionChange={(e) => setDataToImport(e.value)}
                        dataKey="id"
                    >
                        <Column selectionMode="multiple" headerStyle={{ width: '3rem' }}></Column>
                        <Column field="name" header="Account Name" />
                        <Column field="bankName" header="Bank Name" />
                        <Column field="iban" header="IBAN" />
                        <Column field="accountHolder" header="Owner" />
                        <Column field="currencyCode" header="" />
                    </DataTable>
                </div>
            )}

            {jsonData.length === 0 && selectedFile && (
                <p className="text-yellow-500 mt-2">
                    No valid data found in the JSON file.
                </p>
            )}
        </div>
    );
};