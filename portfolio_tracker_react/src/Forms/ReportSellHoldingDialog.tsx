import {
  PlusCircleIcon,
  MinusCircleIcon,
  XMarkIcon,
  InboxArrowDownIcon,
} from "@heroicons/react/24/outline";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Dialog } from "primereact/dialog";
import { InputNumber } from "primereact/inputnumber";
import React, { useEffect, useState } from "react";
import {
  setSelectedReportSellItem,
  setShowReportSellDialog,
  useAppDispatch,
  useAppSelector,
} from "../Store/RootState";
import { reportSellHolding } from "../Store/Thunks";

export interface IReportSellHoldingDialogProps {
  portfolioId: string;
  stockSymbol: string;
  currencyCode: string;
  currencySymbol: string;
}

export const ReportSellHoldingDialog: React.FC<
  IReportSellHoldingDialogProps
> = (props) => {
  const dispatch = useAppDispatch();
  const showReportSellDialog = useAppSelector(
    (x) => x.portfolios.showReportSellDialog
  );
  const selectedReportSellItem = useAppSelector(
    (x) => x.portfolios.selectedReportSellItem
  );

  const { portfolioId, stockSymbol, currencyCode, currencySymbol } = props;
  const [executeDate, setExecuteDate] = useState<Date>(new Date());
  const [price, setPrice] = useState<number>(0);
  const [quantity, setQuantity] = useState<number>(0);
  const [expenses, setExpenses] = useState<number>(0);

  useEffect(() => {
    setExecuteDate(
      new Date(
        selectedReportSellItem?.executeDate.toString() ?? new Date().toString()
      )
    );
    setPrice(selectedReportSellItem?.price ?? 0);
    setQuantity(selectedReportSellItem?.quantity ?? 0);
  }, [selectedReportSellItem]);

  const onHideDialogHandler = () => {
    dispatch(setShowReportSellDialog(false));
    dispatch(setSelectedReportSellItem(null));
  };

  if (!selectedReportSellItem) {
    return null;
  }

  const onSaveHandler = () => {
    const payload = {
      portfolioId,
      stockSymbol,
      holdingId: selectedReportSellItem.id,
      request: {
        price,
        quantity,
        expenses,
        executeDate,
      },
    };

    dispatch(reportSellHolding(payload));
    dispatch(setShowReportSellDialog(false));
    dispatch(setSelectedReportSellItem(null));
  };

  return (
    <Dialog
      header="Report Sell"
      visible={showReportSellDialog}
      onHide={onHideDialogHandler}
    >
      <div className="min-w-[300px] flex flex-col">
        <div className="flex flex-col mb-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Sell Date
          </label>
          <Calendar
            value={executeDate}
            viewDate={executeDate}
            onChange={(e) => setExecuteDate(e.value ?? executeDate)}
            //   showIcon
            dateFormat="dd/mm/yy"
            // icon={() => <CalendarDaysIcon className="size-4" />}
            className="h-10"
          />
        </div>
        <div className="flex flex-col mb-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Price
          </label>
          <InputNumber
            value={price}
            onChange={(e) => setPrice(e.value ?? price)}
            showButtons
            buttonLayout="horizontal"
            min={0}
            step={1}
            minFractionDigits={2}
            maxFractionDigits={2}
            incrementButtonIcon={() => <PlusCircleIcon className="size-7" />}
            decrementButtonIcon={() => <MinusCircleIcon className="size-7" />}
            mode="currency"
            className="h-10"
            currency={currencyCode}
            locale="tr-TR"
          />
        </div>
        <div className="flex flex-col mb-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Quantity
          </label>
          <InputNumber
            value={quantity}
            onChange={(e) => setQuantity(e.value ?? quantity)}
            showButtons
            buttonLayout="horizontal"
            min={1}
            max={selectedReportSellItem.quantity}
            step={1}
            minFractionDigits={0}
            maxFractionDigits={0}
            incrementButtonIcon={() => <PlusCircleIcon className="size-7" />}
            decrementButtonIcon={() => <MinusCircleIcon className="size-7" />}
            mode="decimal"
            className="h-10"
          />
        </div>
        <div className="flex flex-col mb-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Expenses
          </label>
          <InputNumber
            value={expenses}
            onChange={(e) => setExpenses(e.value ?? 0)}
            showButtons
            buttonLayout="horizontal"
            min={0}
            step={1}
            incrementButtonIcon={() => <PlusCircleIcon className="size-7" />}
            decrementButtonIcon={() => <MinusCircleIcon className="size-7" />}
            mode="currency"
            className="h-10"
            currency={currencyCode}
            locale="tr-TR"
          />
        </div>
        <div className="md:col-span-2 flex flex-row-reverse">
          <Button
            className="dark:bg-green p-2 mt-2 text-black"
            onClick={onHideDialogHandler}
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
    </Dialog>
  );
};
