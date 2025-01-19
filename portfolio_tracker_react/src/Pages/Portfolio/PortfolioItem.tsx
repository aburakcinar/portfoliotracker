import React, { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { PlusIcon } from "@heroicons/react/24/outline";
import { useParams } from "react-router";
import {
  fetchHoldingsByPortfolioIdApi,
  IHoldingAggregateModel,
} from "../../Api/HoldingsApi";
import { Card } from "primereact/card";
import { DataView } from "primereact/dataview";
import { IPortfolioModel } from "../../Store";
import { getPortfolioApi } from "../../Api/PortfolioV2Api";
import { getRandomColor } from "../../Tools/getRandomColor";
import { HoldingListItem } from "./HoldingListItem";

export const PortfolioItem: React.FC = () => {
  const { portfolioId } = useParams();
  const [portfolio, setPortfolio] = useState<IPortfolioModel | null>(null);

  const [newInvestmentForm, setNewInvestmentForm] = useState<boolean>(false);
  const [holdings, setHoldings] = useState<IHoldingAggregateModel[]>([]);

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId) {
        const result = await getPortfolioApi(portfolioId);
        setPortfolio(result);

        const resultHoldings = await fetchHoldingsByPortfolioIdApi(portfolioId);
        const resultHoldingWithColor = [
          ...resultHoldings.map((x) => {
            return {
              ...x,
              color: getRandomColor(),
            };
          }),
        ];
        setHoldings(resultHoldingWithColor);
      }
    };
    fetch();
  }, [portfolioId]);

  if (!portfolio) {
    return null;
  }

  const itemTemplate = (item: IHoldingAggregateModel) => {
    return <HoldingListItem item={item} />;
  };

  const { name } = portfolio;

  const listTemplate = (items: IHoldingAggregateModel[]) => {
    if (!items || items.length === 0) return null;

    let list = items.map((product) => {
      return itemTemplate(product);
    });

    return <div className="grid grid-nogutter">{list}</div>;
  };

  return (
    <div className="flex min-w-[500px] w-1/2  mt-5 flex-col ">
      <div className="flex w-full py-2 pt-4">
        <div className="grow">
          <span className="text-green text-4xl py-2">{name}</span>
          <span className="pl-2 text-xl">Portfolio</span>
        </div>
        <div className="grow-0">
          <Button
            className="flex flex-row dark:bg-green p-1 items-center"
            onClick={() => setNewInvestmentForm(true)}
          >
            <PlusIcon className="size-6 text-black" />
            <span className=" text-black">New Holding</span>
          </Button>
        </div>
      </div>

      <Card>
        <DataView value={holdings} listTemplate={listTemplate} />
      </Card>
      {/* <div className="flex bg-highlight rounded-t-md mt-2 w-full">
        <div className="grow p-4">Stock ({currencyCode})</div>
        <div className="w-[100px] p-4">Quantity</div>
        <div className="w-[110px] p-4">Price {currencySymbol}</div>
        <div className="w-[130px] p-4">Total {currencySymbol}</div>
        <div className="w-[40px] p-4">&nbsp;</div>
      </div> */}
      {/* {newInvestmentForm && (
        <div className="flex flex-row w-full">
          <AutoComplete
            field="name"
            className="w-full p-4 grow"
            inputClassName="w-full "
            value={selectedStock}
            suggestions={filteredStocks}
            completeMethod={search}
            onChange={(e: AutoCompleteChangeEvent) => setSelectedStock(e.value)}
            itemTemplate={itemTemplate}
            selectedItemTemplate={itemTemplate}
          />
          <div className="grow-0 py-4 pr-4">
            <Button className="dark:bg-green" onClick={onNewHoldingHandler}>
              Save
            </Button>
          </div>
          <div className="grow-0 py-4 ">
            <Button
              severity="warning"
              onClick={(_) => setNewInvestmentForm(false)}
            >
              Hide
            </Button>
          </div>
        </div>
      )} */}
      {/* {holdings.map((holding) => (
        <Holding
          portfolioId={id}
          currencySymbol={currencySymbol}
          currencyCode={currencyCode}
          holding={holding}
        />
      ))} */}
    </div>
  );
};
