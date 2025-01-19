import React, { useEffect, useState } from "react";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { PlusCircleIcon, PencilSquareIcon } from "@heroicons/react/24/outline";
import classNames from "classnames";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { searchAssets } from "../../Store/AssetsSlice";
import { AssetTypes, IAssetModel } from "../../Api/AssetApi";
import { useLocation, useNavigate } from "react-router";
import { addMenuItem, IMenuItem } from "../../Store";

export default function AssetsStocks() {
  const dispatch = useAppDispatch();
  const stocks = useAppSelector((x) => x.assets.stocks);
  const searchParameters = useAppSelector((x) => x.assets.stocksSearchRequest);

  const location = useLocation(); // Provides the current path
  const navigate = useNavigate();

  const [searchText, setSearchText] = useState<string>(
    searchParameters?.searchText ?? ""
  );
  const [pageIndex, setPageIndex] = useState<number>(
    searchParameters?.pageIndex ?? 0
  );
  const [pageSize, setPageSize] = useState<number>(
    searchParameters?.pageSize ?? 20
  );
  const [editingMode, setEditingMode] = useState<boolean>(false);

  useEffect(() => {
    const request = {
      assetType: AssetTypes.Stock,
      searchText,
      pageIndex,
      pageSize,
    };

    dispatch(searchAssets(request));
  }, [searchText]);

  const onNewStockHandler = () => {
    setEditingMode((prev) => !prev);
  };

  const containerClass = classNames("grid gap-2", {
    "md:grid-cols-2": editingMode,
  });

  const grigColumnBodyTemplate = (item: IAssetModel) => {
    const onClickHandler = () => {
      const tempMenuItem: IMenuItem = {
        id: `${location.pathname}/${item.id}`,
        text: item.name,
        link: `${location.pathname}/${item.id}`,
      };
      dispatch(addMenuItem(tempMenuItem));
      navigate(tempMenuItem.link);
    };

    return (
      <div className="flex flex-row">
        <button onClick={onClickHandler}>
          <PencilSquareIcon className="size-5" />
        </button>
      </div>
    );
  };

  return (
    <div className="flex min-w-[500px] w-1/2  mt-5 flex-col ">
      <h3 className="text-4xl text-green py-2">Stocks</h3>
      <div className={containerClass}>
        <Card className="w-full flex flex-col">
          <label className=" py-1 text-xs dark:text-white text-black">
            Search
          </label>
          <div className="flex gap-2">
            <InputText
              className="grow"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
            />
            <button
              className="grow-0 bg-green p-2"
              title="New Stock"
              onClick={onNewStockHandler}
            >
              <PlusCircleIcon className="size-5" />
            </button>
          </div>
        </Card>
        <div />
        <Card className="w-full">
          <DataTable value={stocks}>
            <Column field="tickerSymbol" header="Ticker" />
            <Column field="exchangeCode" header="Exchange" />
            <Column field="name" header="Name" />
            <Column field="isin" header="ISIN" />
            <Column field="wkn" header="WKN" />
            <Column field="currencyCode" header="Currency" />
            <Column body={grigColumnBodyTemplate} />
          </DataTable>
        </Card>
      </div>
    </div>
  );
}
