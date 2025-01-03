import { useEffect, useState } from "react";
import { IPortfolioModel } from "../Store";
import { useAppDispatch, useAppSelector } from "../Store/RootState";
import {
  fetchPortfolioHoldings,
  reserveStockOnPortfolio,
} from "../Store/Thunks";

import { Holding } from "./Holding";
import { Button } from "primereact/button";
import { PlusIcon } from "@heroicons/react/24/outline";
import {
  AutoComplete,
  AutoCompleteChangeEvent,
  AutoCompleteCompleteEvent,
} from "primereact/autocomplete";
import { IStockItem, listStocks } from "../Api";

interface IPortfolioItemProps {
  portfolio: IPortfolioModel;
}

const PortfolioItem: React.FC<IPortfolioItemProps> = (props) => {
  const { portfolio } = props;
  const { id, name, currencyCode, currencySymbol } = portfolio;
  const [newInvestmentForm, setNewInvestmentForm] = useState<boolean>(false);

  const holdings = useAppSelector((x) => x.portfolios.holdings[id] ?? []);
  const dispatch = useAppDispatch();
  const [selectedStock, setSelectedStock] = useState<IStockItem | null>(null);
  const [stocks, setStocks] = useState<IStockItem[]>([]);
  const [filteredStocks, setFilteredStocks] = useState<IStockItem[]>([]);

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
  };

  useEffect(() => {
    dispatch(fetchPortfolioHoldings(id));
  }, [id]);

  const onNewHoldingHandler = () => {
    if (selectedStock) {
      dispatch(
        reserveStockOnPortfolio({
          portfolioId: id,
          stockSymbol: selectedStock.symbol,
        })
      );
      setNewInvestmentForm(false);
      setSelectedStock(null);
    }
  };

  return (
    <div className="flex flex-col">
      <div className="flex w-full py-2 pt-4">
        <div className="grow">
          <span className="text-green text-xl">{name}</span>
          <span> Portfolio</span>
        </div>
        <div className="grow-0">
          {/* <AddInvestmentPopup currency={currencyCode} portfolioId={id} /> */}
          <Button
            className="flex flex-row dark:bg-green p-1 items-center"
            onClick={() => setNewInvestmentForm(true)}
          >
            <PlusIcon className="size-6 text-black" />
            <span className=" text-black">Investment</span>
          </Button>
        </div>
      </div>
      <div className="flex bg-highlight rounded-t-md mt-2 w-full">
        <div className="grow p-4">Stock ({currencyCode})</div>
        <div className="w-[100px] p-4">Quantity</div>
        <div className="w-[110px] p-4">Price {currencySymbol}</div>
        <div className="w-[130px] p-4">Total {currencySymbol}</div>
        <div className="w-[40px] p-4">&nbsp;</div>
      </div>
      {newInvestmentForm && (
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
      )}
      {holdings.map((holding) => (
        <Holding
          portfolioId={id}
          currencySymbol={currencySymbol}
          currencyCode={currencyCode}
          holding={holding}
        />
      ))}
    </div>
  );
};

export { type IPortfolioItemProps, PortfolioItem };
