import React, { CSSProperties, useEffect, useRef, useState } from "react";
import { InputText } from "primereact/inputtext";
import { OverlayPanel } from "primereact/overlaypanel";
import { classNames } from "primereact/utils";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { ICurrencyItem, getCurrencies } from "../Api";

export interface ICurrencyPickerProps {
  inputClass?: string;
  value: string | null;
  onChange: (value: string | null) => void;
}

export const CurrencyPicker: React.FC<ICurrencyPickerProps> = (props) => {
  const { inputClass, onChange, value } = props;

  const [searchText, setSearchText] = useState<string>("");
  const [isDialogVisible, setIsDialogVisible] = useState<boolean>(true);
  const targetRef = useRef<HTMLDivElement>(null);
  const opRef = useRef<OverlayPanel>(null);
  const [currencies, setCurrencies] = useState<ICurrencyItem[]>([]);
  const [filteredCurrencies, setFilteredCurrencies] = useState<ICurrencyItem[]>(
    []
  );
  const [selectedCurrency, setSelectedCurrency] =
    useState<ICurrencyItem | null>(null);

  useEffect(() => {
    if (opRef.current && targetRef.current) {
      if (isDialogVisible && !opRef.current.isVisible()) {
        opRef.current.show(null, targetRef.current);
      } else if (!isDialogVisible && opRef.current.isVisible()) {
        opRef.current.hide();
      }
    }
  }, [isDialogVisible]);

  useEffect(() => {
    const fetchCurrencies = async () => {
      try {
        const result = await getCurrencies();
        setCurrencies(result);
      } catch (err) {
        console.error(err);
        setCurrencies([]);
      }
    };
    fetchCurrencies();
  }, []);

  useEffect(() => {
    if (value && value !== "") {
      const currency = currencies.find((c) => c.code === value);
      if (currency) {
        setSelectedCurrency(currency);
      }
    }
  }, [value, currencies]);

  useEffect(() => {
    onChange(selectedCurrency?.code ?? null);
  }, [selectedCurrency]);

  useEffect(() => {
    if (searchText.length > 0) {
      setSelectedCurrency(null);
    }

    setIsDialogVisible((_) => searchText.length > 2);

    if (searchText.length > 2) {
      const filtered = currencies.filter(
        (x) =>
          x.code.toLowerCase().includes(searchText.toLowerCase()) ||
          x.name.toLowerCase().includes(searchText.toLowerCase()) ||
          x.nameLocal.toLowerCase().includes(searchText.toLowerCase())
      );
      setFilteredCurrencies(filtered);
    } else {
      setFilteredCurrencies([]);
    }
  }, [searchText]);

  const onRowClickHandler = (item: ICurrencyItem) => {
    setSearchText("");
    setSelectedCurrency(item);
    setIsDialogVisible(false);
  };

  const style: CSSProperties = { width: targetRef.current?.clientWidth ?? 200 };

  return (
    <>
      <div className="flex flex-row h-12" ref={targetRef}>
        <div className="relative grow">
          <InputText
            className={classNames(
              inputClass,
              "absolute w-full h-full left-0 top-0"
            )}
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
          />
          {selectedCurrency && (
            <div className="absolute left-2 top-2">
              {selectedCurrency.code} - {selectedCurrency.name}
            </div>
          )}
        </div>
        <button
          className="flex grow-0 bg-green dark:bg-green hover:bg-green/80 dark:hover:bg-green/80 w-10 text-nav justify-center items-center"
          onClick={(_) => setIsDialogVisible((prev) => !prev)}
        >
          <EllipsisVerticalIcon className="size-6 " />
        </button>
      </div>
      <OverlayPanel ref={opRef} closeOnEscape>
        <div
          className="flex w-full border border-green bg-zinc-600"
          style={style}
        >
          <DataTable
            value={filteredCurrencies}
            className="w-full"
            onRowClick={(e) => onRowClickHandler(filteredCurrencies[e.index])}
            rowClassName={(_) => "hover:bg-highlight dark:hover:bg-highlight"}
          >
            <Column field="code" header="Code" />
            <Column field="name" header="Name" />
            <Column field="symbol" header="Symbol" />
          </DataTable>
        </div>
      </OverlayPanel>
    </>
  );
};
