import React, { useEffect } from "react";
import { useNavigate } from "react-router";
import { PlusIcon } from "@heroicons/react/24/outline";
import { DataTable } from "primereact/datatable";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { fetchBankAccounts } from "../../Store/BankAccount.slice";
import { Column } from "primereact/column";
import { IBankAccountModel } from "../../Api/BankAccount.api";
import { PencilIcon, ArrowUpTrayIcon, QueueListIcon } from "@heroicons/react/24/outline";
import { addMenuItem } from "../../Store";
import { BankAccountDeleteDialog } from "./BankAccountDeleteDialog";

export const BankAccountList: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const banks = useAppSelector((x) => x.bankAccounts.list);

  useEffect(() => {
    dispatch(fetchBankAccounts());
  }, []);

  const onNewBankAccountHandler = () => navigate("new");

  const onImportBankAccountHandler = () => navigate("import");

  const contextMenuTemplate = (item: IBankAccountModel) => {
    const onEditHandler = () => {
      dispatch(
        addMenuItem({
          id: `bankaccounts/${item.id}`,
          text: item.name,
          link: `/bankaccounts/${item.id}`,
        })
      );
      navigate(`${item.id}`);
    };

    const onTranscationHandler = () => { 
      navigate(`/bankaccounts/${item.id}/transactions`);
    };


    return (
      <div className="flex flex-row gap-2">

        <button title="Edit" onClick={onEditHandler}>
          <PencilIcon className="size-5 text-gray-500 hover:text-gray-300" />
        </button>
        <button title="Transactions" onClick={onTranscationHandler}>
          <QueueListIcon className="size-5 text-gray-500 hover:text-gray-300" />
        </button>
        <BankAccountDeleteDialog item={item} onDelete={() => dispatch(fetchBankAccounts())} />
      </div>
    );
  };

  return (
    <div className="flex flex-col min-w-[500px] w-1/2 ">
      <h2 className="text-green pb-8 text-5xl">Bank Accounts</h2>
      <div className="flex flex-row-reverse pb-2">
        <button
          className="bg-green h-10 flex flex-row p-2 "
          onClick={onNewBankAccountHandler}
          title="New Account"
        >
          <PlusIcon className="size-5" />
        </button>
        <button
          className="bg-green h-10 flex flex-row p-2 mr-1"
          onClick={onImportBankAccountHandler}
          title="Import Bank Accounts"
        >
          <ArrowUpTrayIcon className="size-5" />
        </button>
      </div>
      <DataTable value={banks}>
        <Column field="name" header="Account Name" />
        <Column field="bankName" header="Bank Name" />
        <Column field="iban" header="IBAN" />
        <Column field="currencyCode" header="Currency" />
        <Column body={contextMenuTemplate} />
      </DataTable>

    </div>
  );
};
