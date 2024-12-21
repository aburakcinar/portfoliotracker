import { useState } from "react";
import { IPortfolioItem } from "../Api/PortfolioApi";
import { PlusIcon } from "@heroicons/react/24/solid";
import { InvestmentForm } from "./InvestmentForm";

interface IPortfolioItemProps {
  portfolio: IPortfolioItem;
}

const PortfolioItem: React.FC<IPortfolioItemProps> = (props) => {
  const { portfolio } = props;
  const { name, currencyCode, currencySymbol } = portfolio;

  const [showNewInvestmentForm, setShowNewInvestmentForm] =
    useState<boolean>(false);

  return (
    <div className="flex flex-col">
      <div className="flex w-full py-2 pt-4">
        <div className="grow">
          <span className="text-green text-xl">{name}</span>
          <span> Portfolio</span>
        </div>
        <div className="grow-0">
          <button
            className="flex flex-row rounded-md bg-green p-1 items-center"
            onClick={(_) => setShowNewInvestmentForm((prev) => !prev)}
          >
            <PlusIcon className="size-6 text-black" />
            <span className=" text-black">Investment</span>
          </button>
        </div>
      </div>
      <div className="flex bg-highlight rounded-t-md mt-2 w-full">
        <div className="grow p-4">Symbol ({currencyCode})</div>
        <div className="w-[100px] p-4">Amount</div>
        <div className="w-[100px] p-4">Price {currencySymbol}</div>
        <div className="w-[100px] p-4">Total {currencySymbol}</div>
      </div>
      {showNewInvestmentForm && <InvestmentForm />}
    </div>
  );
};

export { type IPortfolioItemProps, PortfolioItem };
