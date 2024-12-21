import React, { useEffect, useState } from "react";
import { PlusIcon } from "@heroicons/react/24/outline";
import CreatePortfolioForm from "../Forms/CreatePortfolioForm";
import { IPortfolioItem, listPortfolios } from "../Api/PortfolioApi";
import { PortfolioItem } from "../Forms/PortfolioItem";

export default function Portfolio() {
  const [showNewPortfolioForm, setShowNewPortfolioForm] =
    useState<boolean>(false);

  const [fetchCounter, setFetchCounter] = useState<number>(0);
  const [portfolios, setPortfolios] = useState<IPortfolioItem[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const data = await listPortfolios();

      console.log("portfolios", data);

      setPortfolios(data);
    };

    fetch();
  }, [fetchCounter]);

  const showCreatePortfolioForm = () => {
    setShowNewPortfolioForm(true);
  };

  const onNewPortfolioHandler = () => {
    console.log("new portfolio");
    setShowNewPortfolioForm(false);
    setFetchCounter((val) => val + 1);
  };

  const onPortfolioFormCloseHandler = () => {
    setShowNewPortfolioForm(false);
  };

  return (
    <div className="pt-16">
      <h2 className="text-green pl-8 text-3xl">Portfolio</h2>

      <div className="flex">
        <div className="flex-auto w-32">&nbsp;</div>
        <div className="flex-auto w-64 rounded-md  ">
          <div className="flex flex-row-reverse">
            {!showNewPortfolioForm && (
              <button
                className="bg-highlight  rounded-md my-2 p-1"
                onClick={showCreatePortfolioForm}
              >
                <PlusIcon className="size-8  text-green " />
              </button>
            )}
          </div>
          {showNewPortfolioForm && (
            <CreatePortfolioForm
              onCreated={onNewPortfolioHandler}
              onClosed={onPortfolioFormCloseHandler}
            />
          )}
          {/* <div className="flex bg-highlight rounded-t-md mt-2">
            <div className="grow p-4">Symbol</div>
            <div className="w-[100px] p-4">Amount</div>
            <div className="w-[100px] p-4">Price</div>
            <div className="w-[100px] p-4">Total</div>
          </div> */}
          {portfolios.map((x) => {
            return <PortfolioItem portfolio={x} />;
          })}
          {/* {examplePortfolio.map((item) => {
            return (
              <div className="flex text-xs bg-nav rounded-b-md">
                <div className="grow p-4 flex-col">
                  <div className="pb-1 ">
                    <span className="bg-red-900 rounded p-1 ">
                      {item.symbol}
                    </span>
                  </div>
                  <div className="text-[9pt] pt-2">{item.totalValue}</div>
                </div>
                <div className="w-[100px] p-4">{item.amount}</div>
                <div className="w-[100px] p-4">
                  <Currency value={item.currentPrice} />
                </div>
                <div className="w-[140px] p-4">
                  <Currency value={item.totalValue} />
                </div>
              </div>
            );
          })} */}
        </div>

        <div className="flex-auto w-32">&nbsp;</div>
      </div>
    </div>
  );
}

// interface IPortfolioModel {
//   symbol: string;
//   currentPrice: number;
//   amount: number;
//   totalValue: number;
// }

// const examplePortfolio: IPortfolioModel[] = [
//   {
//     symbol: "TUPRS",
//     currentPrice: 152.76,
//     amount: 125,
//     totalValue: 18550,
//   },
//   {
//     symbol: "ENJSA",
//     currentPrice: 62.98,
//     amount: 200,
//     totalValue: 7000,
//   },
// ];
