import { getCurrencies, ICurrencyItem } from "../Api/CurrencyApi";
import React, { ChangeEventHandler, useEffect, useState } from "react";
import Dropdown from "../Controls/Dropdown";
import { createPortfolio } from "../Api/PortfolioApi";

interface ICreatePortfolioFormProps {
  onCreated?: () => void;
  onClosed?: () => void;
}

export default function CreatePortfolioForm(
  props: Readonly<ICreatePortfolioFormProps>
) {
  const [portfolioName, setPortfolioName] = useState<string>("");
  const [currencies, setCurrencies] = useState<ICurrencyItem[]>([]);
  const [selecteCurrency, setSelectedCurrency] = useState<string>("Currency");

  useEffect(() => {
    const fetch = async () => {
      const data = await getCurrencies();
      setCurrencies(data);
    };

    fetch();
  }, []);

  const onPortfolioNameChanged = (value: string) => {
    setPortfolioName(value);
  };

  const onSelectHandler = (option: string) => {
    console.log(option);
    setSelectedCurrency(option);
  };

  const onCreatePortfolioHandler = async () => {
    const result = await createPortfolio(portfolioName, selecteCurrency);

    if (result && props.onCreated) {
      props.onCreated();
    }
  };

  const onCloseHandler = () => {
    if (props.onClosed) {
      props.onClosed();
    }
  };

  return (
    <div className="flex flex-col w-full bg-nav rounded-b-md">
      <div className="flex bg-highlight rounded-t-md w-full">
        <div className=" p-4 ">New Portfolio</div>
      </div>
      <div className="flex h-14">
        <div className="w-[120px] my-3 mx-3">Name</div>
        <input
          type="text"
          className="w-64 h-8 my-4 text-black"
          value={portfolioName}
          onChange={(e) => onPortfolioNameChanged(e.currentTarget.value)}
        />
      </div>
      <div className="flex h-14">
        <div className="w-[120px] my-4 mx-3">Currency</div>

        <Dropdown
          label={selecteCurrency}
          onSelect={onSelectHandler}
          options={currencies.map((x) => x.code)}
        />
      </div>
      <div className="flex h-14 flex-row-reverse">
        <button
          className="text-black font-bold bg-highlight h-8 w-24 m-2"
          onClick={onCloseHandler}
        >
          Close
        </button>
        <button
          className="text-black font-bold bg-green h-8 w-24 m-2"
          onClick={onCreatePortfolioHandler}
        >
          Create
        </button>
      </div>
    </div>
  );
}
