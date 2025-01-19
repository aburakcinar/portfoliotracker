import React, { useEffect, useRef, useState } from "react";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { PlusCircleIcon } from "@heroicons/react/24/outline";
import classNames from "classnames";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { StockAddUpdateForm } from "../../Forms/Stocks/StockAddUpdateForm";

export default function AssetsEtfs() {
  const dispatch = useAppDispatch();
  const stocks = useAppSelector((x) => x.stocks.list);
  const [searchText, setSearchText] = useState<string>("");
  const debounceTimer = useRef<ReturnType<typeof setTimeout> | null>(null); // Timer reference
  const [editingMode, setEditingMode] = useState<boolean>(true);

  useEffect(() => {
    const fetch = async () => {};

    // Reset debounce timer
    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }

    // Start new debounce timer
    debounceTimer.current = setTimeout(async () => {
      fetch();
    }, 500);
  }, [searchText]);

  useEffect(() => {
    //dispatch(fetchStockItems());
  }, []);

  const onNewStockHandler = () => {
    setEditingMode((prev) => !prev);
  };

  const containerClass = classNames("grid", { "md:grid-cols-2": editingMode });

  return (
    <div className="flex w-full justify-center mt-5">
      <div className={containerClass}>
        <Card className="m-2 min-w-[600px]">
          <div className="grid gap-2 grid-cols-1 md:grid-cols-2 w-full">
            <div className="flex col-span-2 mb-2">
              <h3 className="grow text-4xl text-green ">ETFs</h3>
              <div className="grow-0">
                <button
                  className=" bg-green p-2"
                  title="New Stock"
                  onClick={onNewStockHandler}
                >
                  <PlusCircleIcon className="size-5" />
                </button>
              </div>
            </div>
            <div className="flex flex-col col-span-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Search
              </label>
              <InputText
                value={searchText}
                onChange={(e) => setSearchText(e.target.value)}
              />
            </div>
          </div>
        </Card>
        <div></div>
        <Card className="m-2 min-w-[600px]">
          <DataTable value={stocks}>
            <Column field="fullCode" header="Code" />
            <Column field="name" header="Name" />
            <Column field="localeCode" header="Country" />
            <Column field="currencyCode" header="Currency" />
          </DataTable>
        </Card>
        {editingMode && (
          <div className="w-full">
            <Card className="m-2">
              <StockAddUpdateForm />
            </Card>
          </div>
        )}
      </div>
    </div>
  );
}
