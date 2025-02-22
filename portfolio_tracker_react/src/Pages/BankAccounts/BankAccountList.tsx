import React, { useEffect } from "react";
import { useNavigate } from "react-router";
import { PlusIcon } from "@heroicons/react/24/outline";
import { DataTable } from "primereact/datatable";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { fetchBankAccounts } from "../../Store/BankAccount.slice";
import { Column } from "primereact/column";
import { IBankAccountModel } from "../../Api/BankAccount.api";
import { PencilIcon } from "@heroicons/react/24/outline";
import { addMenuItem } from "../../Store";

export const BankAccountList: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const banks = useAppSelector((x) => x.bankAccounts.list);

  useEffect(() => {
    dispatch(fetchBankAccounts());
  }, []);

  const onNewBankAccountHandler = () => {
    navigate("new");
  };

  const contextMenuTemplate = (item: IBankAccountModel) => {
    const onClickHandler = () => {
      dispatch(
        addMenuItem({
          id: `bankaccounts/detail/${item.id}`,
          text: item.name,
          link: `/bankaccounts/detail/${item.id}`,
        })
      );
      navigate(`detail/${item.id}`);
    };

    return (
      <div>
        <button onClick={onClickHandler}>
          <PencilIcon className="size-5" />
        </button>
      </div>
    );
  };

  return (
    <div className="flex flex-col min-w-[500px] w-1/2 ">
      <h2 className="text-green pb-8 text-5xl">Bank Accounts</h2>
      <div className="flex flex-row-reverse pb-2">
        <button
          className="bg-green h-10 flex flex-row p-2"
          onClick={onNewBankAccountHandler}
        >
          <PlusIcon className="size-5" />
          <span>New Account</span>
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
