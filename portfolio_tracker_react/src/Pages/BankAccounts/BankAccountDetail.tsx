import React, { useEffect, useState } from "react";
import { useParams } from "react-router";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { Card } from "primereact/card";
import { fetchBankAccounts } from "../../Store/BankAccount.slice";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { AddTransactionForm } from "./AddTransactionForm";
import { addMenuItem } from "../../Store";
import { ListTransactionsForm } from "./ListTransactionsForm";
import { classNames } from "primereact/utils";

export const BankAccountDetail: React.FC = () => {
  const { id } = useParams();
  const item = useAppSelector((x) =>
    x.bankAccounts.list.find((p) => p.id === id)
  );
  const dispatch = useAppDispatch();
  const [showAddNewTransaction, setShowAddNewTransaction] =
    useState<boolean>(true);

  useEffect(() => {
    if (!item) {
      dispatch(fetchBankAccounts());
    } else {
      dispatch(
        addMenuItem({
          id: `bankaccounts/detail/${item.id}`,
          text: item.name,
          link: `/bankaccounts/detail/${item.id}`,
        })
      );
    }
  }, [item]);

  return (
    <div className="flex flex-col w-full p-3">
      <h3 className="text-green text-4xl py-5 pb-10">{item?.name}</h3>
      <div className="grid grid-cols-2 gap-3 ">
        <Card title="Account Information">
          <div className="flex w-full flex-col">
            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Account Name
              </label>
              <InputText value={item?.name} disabled className="h-10" />
            </div>

            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Bank Name
              </label>
              <InputText value={item?.bankName} disabled className="h-10" />
            </div>

            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Account Holder
              </label>
              <InputText
                value={item?.accountHolder}
                disabled
                className="h-10"
              />
            </div>

            <div className="grid grid-cols-2 gap-2">
              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  IBAN
                </label>
                <InputText value={item?.iban} disabled className="h-10" />
              </div>
              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  Open Date
                </label>
                <InputText
                  value={item?.openDate?.toString()}
                  disabled
                  className="h-10"
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-2">
              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  Currency
                </label>
                <InputText
                  value={item?.currencyCode}
                  disabled
                  className="h-10"
                />
              </div>

              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  Country
                </label>
                <InputText value={item?.localeCode} disabled className="h-10" />
              </div>
            </div>

            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Description
              </label>
              <InputTextarea value={item?.description} className="h-36" />
            </div>
          </div>
        </Card>
        <div className="flex flex-col gap-2">
          <Card
            className={classNames({ "h-20": !showAddNewTransaction })}
            title={
              <div className="flex flex-row">
                <div className="grow">Add New Transaction</div>
                <div className="grow-0">
                  <button
                    onClick={(_) => setShowAddNewTransaction((prev) => !prev)}
                  >
                    {showAddNewTransaction ? "Collapse" : "Expand"}
                  </button>
                </div>
              </div>
            }
          >
            {showAddNewTransaction && item && (
              <AddTransactionForm
                bankAccountId={item.id}
                currencyCode={item.currencyCode}
              />
            )}
          </Card>

          <Card
            title="Transactions"
            className={classNames({ "h-full": !showAddNewTransaction })}
          >
            {item && (
              <ListTransactionsForm
                bankAccountId={item.id}
                currencyCode={item.currencyCode}
                currencySymbol={item.currencyCode}
              />
            )}
          </Card>
        </div>
      </div>
    </div>
  );
};
