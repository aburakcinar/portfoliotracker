import {
  AutoComplete,
  AutoCompleteChangeEvent,
  AutoCompleteCompleteEvent,
} from "primereact/autocomplete";
import React, { useEffect, useState } from "react";
import { IStockItem, listStocks } from "../Api/StockApi";
import {
  InputNumber,
  InputNumberValueChangeEvent,
} from "primereact/inputnumber";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Nullable } from "primereact/ts-helpers";
import {
  CalendarDaysIcon,
  PlusCircleIcon,
  MinusCircleIcon,
} from "@heroicons/react/24/outline";

interface IAddInvestmentControlProps {
  portfolioId: string;
  currency: string;
}

const AddInvestmentControl: React.FC<IAddInvestmentControlProps> = (props) => {
  const { currency } = props;

  const [selectedStock, setSelectedStock] = useState<string>("");
  const [stocks, setStocks] = useState<IStockItem[]>([]);
  const [filteredStocks, setFilteredStocks] = useState<IStockItem[]>([]);
  const [quantity, setQuantity] = useState<number>(0);
  const [price, setPrice] = useState<number>(0);
  const [date, setDate] = useState<Nullable<Date>>(null);
  const [expense, setExpense] = useState<number>(0);

  useEffect(() => {
    const fetch = async () => {
      const result = await listStocks();

      setStocks(result);
    };

    fetch();
  }, []);

  const itemTemplate = (item: IStockItem) => {
    return (
      <div className="flex flex-col">
        <div className="grow-0">{item.symbol}</div>
        <div className="grow text-xs">{item.name}</div>
      </div>
    );
  };

  const search = (event: AutoCompleteCompleteEvent) => {
    // Timeout to emulate a network connection
    //setTimeout(() => {
    let _filteredStocks;

    if (!event.query.trim().length) {
      _filteredStocks = [...stocks];
    } else {
      _filteredStocks = stocks.filter((stock) => {
        return (
          stock.name.toLowerCase().startsWith(event.query.toLowerCase()) ||
          stock.symbol.toLowerCase().startsWith(event.query.toLowerCase())
        );
      });
    }

    setFilteredStocks(_filteredStocks);
    //}, 250);
  };

  const onSaveHandler = () => {};

  return (
    <div className="flex flex-col w-full">
      <AutoComplete
        field="name"
        className="w-full p-4"
        inputClassName="w-full "
        value={selectedStock}
        suggestions={filteredStocks}
        completeMethod={search}
        onChange={(e: AutoCompleteChangeEvent) => setSelectedStock(e.value)}
        itemTemplate={itemTemplate}
        selectedItemTemplate={itemTemplate}
      />
      <div className="flex py-1">
        <div className="w-[100px] p-2 dark:text-white text-black">Quantity</div>
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
        />
      </div>
      <div className="flex py-1">
        <div className="w-[100px] p-2 dark:text-white text-black">Buy Date</div>
        <Calendar
          value={date}
          onChange={(e) => setDate(e.value)}
          //   showIcon
          dateFormat="dd/mm/yy"
          // icon={() => <CalendarDaysIcon className="size-4" />}
        />
      </div>
      <div className="flex py-1">
        <div className="w-[100px] p-2 dark:text-white text-black">Price</div>
        <InputNumber
          mode="currency"
          currency={currency}
          value={price}
          onChange={(e) => setPrice(e.value ?? 0)}
        />
      </div>
      <div className="flex py-1">
        <div className="w-[100px] p-2 dark:text-white text-black">Expense</div>
        <InputNumber
          mode="currency"
          currency={currency}
          value={expense}
          onChange={(e) => setExpense(e.value ?? 0)}
        />
      </div>
      <div className="flex flex-row-reverse">
        <Button className="bg-green p-2 text-black w-24">Save</Button>
      </div>
    </div>
  );
};

export { type IAddInvestmentControlProps, AddInvestmentControl };
