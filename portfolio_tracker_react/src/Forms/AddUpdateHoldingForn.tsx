import {
  MinusCircleIcon,
  PlusCircleIcon,
  InboxArrowDownIcon,
  XMarkIcon,
} from "@heroicons/react/24/outline";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import {
  InputNumber,
  InputNumberValueChangeEvent,
} from "primereact/inputnumber";
import { Nullable } from "primereact/ts-helpers";
import React, { useEffect, useState } from "react";
import { IHoldingPurchaseModel } from "../Store";
import { useAppDispatch } from "../Store/RootState";
import { reportBuyStock, updateHolding } from "../Store/Thunks";

export interface IAddUpdateHoldingFormProps {
  portfolioId: string;
  holding: IHoldingPurchaseModel | null;
  currencySymbol: string;
  currencyCode: string;
  stockSymbol: string;
  onClose?: () => void;
}

const getDate = (val: Date | null | undefined): Date => {
  if (val) {
    return new Date(val.toString());
  }

  return new Date();
};

export const AddUpdateHoldingForm: React.FC<IAddUpdateHoldingFormProps> = (
  props
) => {
  const dispatch = useAppDispatch();
  const { portfolioId, holding, currencyCode, stockSymbol, onClose } = props;
  const [quantity, setQuantity] = useState<number>(holding?.quantity ?? 0);
  const [price, setPrice] = useState<number>(holding?.price ?? 0);
  const [executeDate, setExecuteDate] = useState<Nullable<Date>>(
    getDate(holding?.executeDate)
  );
  const [expenses, setExpenses] = useState<number>(holding?.expenses ?? 0);
  const isUpdate = props.holding !== null;

  useEffect(() => {
    setQuantity(holding?.quantity ?? 0);
    setPrice(holding?.price ?? 0);
    setExpenses(holding?.expenses ?? 0);
    setExecuteDate(getDate(holding?.executeDate));
  }, [holding]);

  const onSaveHandler = () => {
    if (isUpdate && holding) {
      dispatch(
        updateHolding({
          portfolioId,
          stockSymbol,
          holdingId: holding.id,
          executeDate: executeDate ?? undefined,
          expenses,
          price,
          quantity,
        })
      );
    } else {
      dispatch(
        reportBuyStock({
          portfolioId,
          stockSymbol,
          quantity,
          price,
          expenses,
          executeDate: executeDate ?? new Date(),
        })
      );
    }

    onCloseHandler();
  };

  const onCloseHandler = () => {
    if (onClose) {
      onClose();
    }
  };

  return (
    <div className="grid gap-2 grid-cols-1 md:grid-cols-2 w-full">
      <div className="flex flex-col ">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Quantity
        </label>
        <InputNumber
          value={quantity}
          onValueChange={(e: InputNumberValueChangeEvent) =>
            setQuantity(e.value ?? 0)
          }
          showButtons
          buttonLayout="horizontal"
          min={0}
          step={1}
          incrementButtonIcon={() => <PlusCircleIcon className="size-7" />}
          decrementButtonIcon={() => <MinusCircleIcon className="size-7" />}
          mode="decimal"
          className="h-10"
        />
      </div>
      <div className="flex flex-col ">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Price
        </label>
        <InputNumber
          mode="currency"
          currency={currencyCode}
          value={price}
          className="h-10"
          onChange={(e) => setPrice(e.value ?? 0)}
        />
      </div>
      <div className="flex flex-col ">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Buy Date
        </label>
        <Calendar
          value={executeDate}
          onChange={(e) => setExecuteDate(e.value)}
          //   showIcon
          dateFormat="dd/mm/yy"
          viewDate={executeDate}
          // icon={() => <CalendarDaysIcon className="size-4" />}
          className="h-10"
        />
      </div>

      <div className="flex flex-col ">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Expense
        </label>
        <InputNumber
          mode="currency"
          currency={currencyCode}
          value={expenses}
          className="h-10"
          onChange={(e) => setExpenses(e.value ?? 0)}
        />
      </div>
      <div className="md:col-span-2 flex flex-row-reverse">
        <Button
          className="dark:bg-green p-2 mt-2 text-black "
          onClick={onCloseHandler}
        >
          <XMarkIcon className="size-5 " />
          <span className="px-2">Close</span>
        </Button>
        <Button
          className="dark:bg-green p-2 mt-2 text-black "
          onClick={onSaveHandler}
        >
          <InboxArrowDownIcon className="size-5 " />
          <span className="px-2">Save</span>
        </Button>
      </div>
    </div>
  );
};
