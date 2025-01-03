import React, { useEffect, useRef, useState } from "react";
import { Card } from "primereact/card";
import {
  AutoComplete,
  AutoCompleteChangeEvent,
  AutoCompleteCompleteEvent,
} from "primereact/autocomplete";
import {
  ISearchStockRequest,
  IStockItem,
  listCountries,
  listCurrencies,
  searchStocks,
} from "../Api/AssetApi";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

export default function Stocks() {
  const [countries, setCountries] = useState<string[]>([]);
  const [filteredCountries, setFilteredCountries] = useState<string[]>([]);
  const [currencies, setCurrencies] = useState<string[]>([]);
  const [filteredCurrencies, setFilteredCurrencies] = useState<string[]>([]);
  const [selectedCounties, setSelectedCountries] = useState<string[]>([]);
  const [selectedCurrencies, setSelectedCurrencies] = useState<string[]>([]);

  const [stocks, setStocks] = useState<IStockItem[]>([]);
  const [searchText, setSearchText] = useState<string>("");
  const debounceTimer = useRef<ReturnType<typeof setTimeout> | null>(null); // Timer reference

  useEffect(() => {
    const fetch = async () => {
      const fetchedCountries = await listCountries();
      const fetchedCurrencies = await listCurrencies();

      setCountries(fetchedCountries);
      setCurrencies(fetchedCurrencies);
    };

    fetch();
  }, []);

  useEffect(() => {
    const fetch = async () => {
      if (searchText === "") {
        setStocks([]);
        return;
      }
      const request: ISearchStockRequest = {
        searchText,
        pageIndex: 0,
        pageSize: 100,
        countries: selectedCounties,
      };
      try {
        const result = await searchStocks(request);

        setStocks(result);
      } catch (err) {
        console.log(err);
      }
    };

    // Reset debounce timer
    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }

    // Start new debounce timer
    debounceTimer.current = setTimeout(async () => {
      fetch();
    }, 500);
  }, [searchText]);

  const searchCountries = ($event: AutoCompleteCompleteEvent) => {
    if ($event.query.trim().length === 0) {
      setFilteredCountries([...countries]);
    } else {
      setFilteredCountries([
        ...countries.filter((x) =>
          x.toLocaleLowerCase().startsWith($event.query.toLowerCase())
        ),
      ]);
    }
  };

  const searchCurrencies = ($event: AutoCompleteCompleteEvent) => {
    if ($event.query.trim().length === 0) {
      setFilteredCurrencies([...currencies]);
    } else {
      setFilteredCurrencies([
        ...currencies.filter((x) =>
          x.toLocaleLowerCase().startsWith($event.query.toLowerCase())
        ),
      ]);
    }
  };

  return (
    <div className="flex w-full justify-center">
      <div className="flex flex-col w-2/4 min-w-[600px]">
        <h3 className="text-5xl text-green p-4">Stocks</h3>
        <Card className="m-2">
          <div className="grid gap-2 grid-cols-1 md:grid-cols-2 w-full">
            <div className="flex flex-col col-span-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Search
              </label>
              <InputText
                value={searchText}
                onChange={(e) => setSearchText(e.target.value)}
              />
            </div>
            {/* <div className="flex flex-col ">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Countries
              </label>
              <AutoComplete
                value={selectedCounties}
                suggestions={filteredCountries}
                completeMethod={searchCountries}
                onChange={(e: AutoCompleteChangeEvent) =>
                  setSelectedCountries(e.value)
                }
                dropdown
                multiple
              />
            </div>
            <div className="flex flex-col ">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Currencies
              </label>
              <AutoComplete
                value={selectedCurrencies}
                suggestions={filteredCurrencies}
                completeMethod={searchCurrencies}
                onChange={(e: AutoCompleteChangeEvent) =>
                  setSelectedCurrencies(e.value)
                }
                dropdown
                multiple
              />
            </div> */}
          </div>
        </Card>
        <Card className="m-2">
          <DataTable value={stocks}>
            <Column field="country" header="Country" />
            <Column field="symbol" header="Symbol" />
            <Column field="name" header="Name" />
            <Column field="fullName" header="FullName" />
            <Column field="isin" header="ISIN" />
            <Column field="currency" header="Currency" />
          </DataTable>
        </Card>
      </div>
    </div>
  );
}
