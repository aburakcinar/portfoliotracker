import React, { useEffect, useState } from "react";
import Dropdown from "../Controls/Dropdown";
import { IStockItem, listStocks } from "../Api/StockApi";

interface IInvestmentFormProps {}

const InvestmentForm: React.FC<IInvestmentFormProps> = (props) => {
  const [symbols, setSymbols] = useState<IStockItem[]>([]);
  const [selectedSymbol, setSelectedSymbol] = useState<string>("Symbol");

  useEffect(() => {
    const fetch = async () => {
      const data = await listStocks();

      setSymbols(data);
    };

    fetch();
  }, []);

  const onSymbolSelectionHandler = (option: string) => {
    setSelectedSymbol(option);
  };

  return (
    <div className="w-full flex flex-col bg-nav">
      <div className="p-1">
        <Dropdown
          className="w-full"
          label={selectedSymbol}
          onSelect={onSymbolSelectionHandler}
          options={symbols.map((x) => x.symbol)}
        />
      </div>
    </div>
  );
};

export { type IInvestmentFormProps, InvestmentForm };
