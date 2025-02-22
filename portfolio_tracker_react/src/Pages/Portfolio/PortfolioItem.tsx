import React, { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { PlusIcon, PencilIcon } from "@heroicons/react/24/outline";
import { useNavigate, useParams } from "react-router";
import { IHoldingAggregateModel } from "../../Api/HoldingsApi";
import { Card } from "primereact/card";
import { DataView } from "primereact/dataview";
import { HoldingListItem } from "./HoldingListItem";
import { useMenuItem } from "../../Hooks/useMenuItem";
import {
  useHoldings,
  usePortfolio,
  usePortfolioTotalPosition,
} from "../../Hooks";

export const PortfolioItem: React.FC = () => {
  const { portfolioId } = useParams();
  const navigate = useNavigate();

  const [iteration, setIteration] = useState<number>(0);
  const portfolio = usePortfolio(portfolioId);
  const portfolioTotal = usePortfolioTotalPosition(portfolioId, [iteration]);
  const holdings = useHoldings(portfolioId, [iteration]);

  useMenuItem(
    {
      id: `portfolios/${portfolioId}`,
      link: `/portfolios/${portfolioId}`,
      text: portfolio?.name ?? "",
      visible: false,
    },
    [portfolio]
  );

  if (!portfolio) {
    return null;
  }

  const itemTemplate = (item: IHoldingAggregateModel) => {
    return <HoldingListItem key={item.assetId} item={item} />;
  };

  const addNewHoldingHandler = () => {
    navigate(`/portfolios/${portfolioId}/addholding`);
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
        <div className="grow-0 flex flex-row">
          <button
            className="flex flex-row  m-1 items-center"
            title="Edit Portfolio"
            onClick={() => {
              navigate(`/portfolios/${portfolioId}/edit`);
            }}
          >
            <PencilIcon className="size-8 text-green/70 hover:text-green" />
          </button>
          <button
            className="flex flex-row m-1 items-center  align-middle "
            title="Add Holding"
            onClick={addNewHoldingHandler}
          >
            <PlusIcon className="size-8 text-green/70 hover:text-green" />
          </button>
        </div>
      </div>
      <div className="grid grid-cols-3 gap-1 mb-5">
        <Card title="Total Positions">
          <div className="text-2xl text-green">
            {portfolioTotal?.currencySymbol}
            {portfolioTotal?.totalPosition}
          </div>
        </Card>
        <Card title="Total Cost">
          <div className="text-2xl text-green">
            {portfolioTotal?.currencySymbol}
            {portfolioTotal?.totalCost}
          </div>
        </Card>
        <Card title="Total Expenses">
          <div className="text-2xl text-green">
            {portfolioTotal?.currencySymbol}
            {portfolioTotal?.totalExpenses}
          </div>
        </Card>
      </div>

      <Card title="Holdings">
        <DataView
          value={holdings}
          listTemplate={listTemplate}
          emptyMessage="There is no holding."
        />
      </Card>
    </div>
  );
};
