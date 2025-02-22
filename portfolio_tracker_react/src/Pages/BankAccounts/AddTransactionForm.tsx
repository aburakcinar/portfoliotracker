import React, { useState } from "react";
import {
  ITransactionActionType,
  TransactionActionTypeCategory,
} from "../../Api/Transaction.api";
import { classNames } from "primereact/utils";
import { Dropdown } from "primereact/dropdown";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { DataTable } from "primereact/datatable";
import {
  addTransaction,
  IBankAccountTransactionCreateModel,
} from "../../Store/Transaction.slice";
import { Column } from "primereact/column";
import { IAddTransactionCommand, InOut } from "../../Api/BankTransactions.api";
import { produce } from "immer";
import {
  InputNumber,
  InputNumberValueChangeEvent,
} from "primereact/inputnumber";
import {
  MinusCircleIcon,
  PlusCircleIcon,
  TrashIcon,
} from "@heroicons/react/24/outline";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Nullable } from "primereact/ts-helpers";

export interface IAddTransactionFormProps {
  bankAccountId: string;
  currencyCode: string;
}

export const AddTransactionForm: React.FC<IAddTransactionFormProps> = (
  props
) => {
  const actionTypes = useAppSelector((x) => x.transactions.actionTypes);
  const actionTypeCategories = useAppSelector(
    (x) => x.transactions.actionTypeCategories
  );
  const [operationDate, setOperationDate] = useState<Date>(new Date());

  const { bankAccountId, currencyCode } = props;

  const dispatch = useAppDispatch();

  const [transactions, setTransactions] = useState<
    IBankAccountTransactionCreateModel[]
  >([]);

  const renderCategories = () => {
    const onClickHandler = (item: {
      label: string;
      category: TransactionActionTypeCategory;
    }) => {
      setTransactions((prev) => {
        return [
          ...prev,
          {
            index: Math.max(...prev.map((x) => x.index), -1) + 1,
            price: 0,
            quantity: 0,
            inOut:
              item.category === TransactionActionTypeCategory.Incoming
                ? InOut.In
                : InOut.Out,
            actionTypeCode: "",
            actionTypeCategory: item.category,
            description: "",
          } satisfies IBankAccountTransactionCreateModel,
        ];
      });
    };

    return (
      <div className="grid grid-cols-4 w-full gap-1">
        {actionTypeCategories.map((item) => {
          return (
            <button
              key={item.label}
              onClick={(_) => onClickHandler(item)}
              className={classNames(
                "text-black/60 p-2 font-semibold h-10",
                "bg-green/60 hover:bg-green"
              )}
            >
              {item.label}
            </button>
          );
        })}
      </div>
    );
  };

  const headerTemplate = (data: IBankAccountTransactionCreateModel) => {
    return (
      <div className="flex align-items-center gap-2">
        <span className="font-bold">
          {getCategory(data.actionTypeCategory)}
        </span>
      </div>
    );
  };

  const getCategory = (category: TransactionActionTypeCategory): string => {
    return (
      actionTypeCategories.find((x) => x.category === category)?.label ?? ""
    );
  };

  const getInOut = (value: InOut): string => {
    if (value === InOut.Out) {
      return "Out";
    }
    return "In";
  };

  const inOutItemTemplate = (item: IBankAccountTransactionCreateModel) => {
    return getInOut(item.inOut);
  };

  const actionTypeCodeItemTemplate = (
    item: IBankAccountTransactionCreateModel
  ) => {
    const setSelectedActionTypeCode = (code: string) => {
      setTransactions((prev) => {
        return produce(prev, (draftState) => {
          const found = draftState.find((x) => x.index === item.index);

          if (found) {
            found.actionTypeCode = code;
          }
        });
      });
    };

    return (
      <Dropdown
        options={actionTypes.filter(
          (x) => x.category === item.actionTypeCategory
        )}
        value={item.actionTypeCode}
        optionValue="code"
        optionLabel="name"
        className="h-10 w-30"
        onChange={(e) => setSelectedActionTypeCode(e.value)}
      />
    );
  };

  const priceItemTemplate = (item: IBankAccountTransactionCreateModel) => {
    const setPriceValue = (value: number | null) => {
      if (!value) {
        return;
      }

      setTransactions((prev) => {
        return produce(prev, (draftState) => {
          const found = draftState.find((x) => x.index === item.index);

          if (found) {
            found.price = value;
          }
        });
      });
    };

    return (
      <InputNumber
        mode="currency"
        currency={currencyCode}
        value={item.price}
        className="h-10 w-[120px]"
        onChange={(e) => setPriceValue(e.value ?? 0)}
      />
    );
  };

  const quantityItemTemplate = (item: IBankAccountTransactionCreateModel) => {
    const setQuantityValue = (value: number | null) => {
      if (!value) {
        return;
      }

      setTransactions((prev) => {
        return produce(prev, (draftState) => {
          const found = draftState.find((x) => x.index === item.index);

          if (found) {
            found.quantity = value;
          }
        });
      });
    };

    return (
      <InputNumber
        value={item.quantity}
        onValueChange={(e: InputNumberValueChangeEvent) =>
          setQuantityValue(e.value ?? 0)
        }
        showButtons
        buttonLayout="horizontal"
        min={0}
        step={1}
        incrementButtonIcon={() => <PlusCircleIcon className="size-7" />}
        decrementButtonIcon={() => <MinusCircleIcon className="size-7" />}
        mode="decimal"
        className="h-10 w-[140px]"
      />
    );
  };

  const contextMenuItemTemplate = (
    item: IBankAccountTransactionCreateModel
  ) => {
    const removeHandler = () => {
      setTransactions((prev) => {
        return prev.filter((x) => x.index !== item.index);
      });
    };

    return (
      <button onClick={removeHandler}>
        <TrashIcon className="size-5 text-orange-700 hover:text-orange-500" />
      </button>
    );
  };

  const subTotalItemTemplate = (item: IBankAccountTransactionCreateModel) => {
    const subTotal =
      (item.inOut === InOut.Out ? -1 : 1) * item.price * item.quantity;

    const className = classNames(
      "h-10 w-[120px]",
      { "text-emerald-500": item.inOut === InOut.In },
      { "text-red-600": item.inOut === InOut.Out }
    );
    const sign = item.inOut === InOut.Out ? "-" : "+";

    return (
      <div className={className}>
        <InputNumber
          mode="currency"
          currency={currencyCode}
          value={subTotal}
          className={className}
          readOnly={true}
          inputClassName="text-right"
        />
      </div>
    );
  };

  const calculateTotal = () => {
    return transactions.reduce(
      (prev, next) =>
        prev + (next.inOut === InOut.In ? 1 : -1) * next.price * next.quantity,
      0
    );
  };

  const footer = () => {
    return (
      <div className="flex flex-row-reverse">
        <InputNumber
          value={calculateTotal()}
          className="border-0 mr-7 w-[120px] bg-transparent dark:bg-transparent"
          mode="currency"
          currency={currencyCode}
          readOnly={true}
          inputClassName="text-right "
        />
      </div>
    );
  };

  const onSaveButton = () => {
    const command = {
      bankAccountId,
      operationDate,
      transactions,
    } satisfies IAddTransactionCommand;

    dispatch(addTransaction(command));
  };

  return (
    <div className="flex flex-col">
      <div className="grid grid-cols-2 gap-2">
        <div>
          <div className="content-center">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Transaction Date
            </label>
          </div>
          <Calendar
            value={operationDate}
            onChange={(e) => setOperationDate(e.value ?? new Date())}
            //   showIcon
            dateFormat="dd/mm/yy"
            viewDate={operationDate}
            className="h-10"
          />
        </div>

        <div className="flex flex-col">
          <div className="content-center">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Add Transaction by Category
            </label>
          </div>
          {renderCategories()}
        </div>
      </div>
      <DataTable
        value={transactions}
        className="pt-2"
        sortField="actionTypeCategory"
        groupRowsBy="actionTypeCategory"
        rowGroupMode="subheader"
        rowGroupHeaderTemplate={headerTemplate}
        size="small"
        footer={footer}
        showHeaders={false}
      >
        <Column
          field="actionTypeCode"
          header="ActionTypeCode"
          body={actionTypeCodeItemTemplate}
        />
        <Column field="price" header="Amount" body={priceItemTemplate} />
        <Column
          field="quantity"
          header="Quantity"
          body={quantityItemTemplate}
        />
        <Column className="w-full" />
        <Column header="Total" body={subTotalItemTemplate} />
        <Column className="p-1 max-w-20" body={contextMenuItemTemplate} />
      </DataTable>
      <div className="flex flex-row-reverse pt-10">
        <Button
          className="w-50 h-12 bg-green dark:bg-green hover:bg-green/70 dark:hover:bg-green/70"
          onClick={onSaveButton}
          disabled={transactions.length === 0}
        >
          Save
        </Button>
      </div>
    </div>
  );
};
