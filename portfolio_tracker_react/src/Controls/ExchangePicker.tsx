import { OverlayPanel } from "primereact/overlaypanel";
import React, { CSSProperties, useEffect, useRef, useState } from "react";
import {
  getExchangeApi,
  IExchangeQueryModel,
  searchExchangesApi,
} from "../Api/Exchange.api";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import classNames from "classnames";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { InputText } from "primereact/inputtext";

export interface IExchangePickerProps {
  inputClass?: string;
  value: string | null;
  onChange: (e: any) => void;
}

export const ExchangePicker: React.FC<IExchangePickerProps> = (props) => {
  const { inputClass, onChange, value } = props;

  const [searchText, setSeachText] = useState<string>("");
  const [isDialogVisible, setIsDialogVisible] = useState<boolean>(false);
  const targetRef = useRef<HTMLDivElement>(null);
  const opRef = useRef<OverlayPanel>(null);

  const [exchanges, setExchanges] = useState<IExchangeQueryModel[]>([]);
  const [selectedExchange, setSelectedExchange] =
    useState<IExchangeQueryModel | null>(null);

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
    onChange(selectedExchange?.code ?? "");
  }, [selectedExchange?.mic]);

  useEffect(() => {
    if (selectedExchange) {
      const fetch = async () => {
        try {
          const result = await getExchangeApi(selectedExchange.mic);

          setSelectedExchange(result);
        } catch (err) {
          console.log(err);
          setSelectedExchange(null);
        }
      };
      fetch();
    }
  }, [value]);

  useEffect(() => {
    if (searchText.length > 0) {
      setSelectedExchange(null);
    }

    setIsDialogVisible((_) => searchText.length > 2);

    if (searchText.length > 2) {
      const fetch = async () => {
        try {
          const result = await searchExchangesApi(searchText);

          setExchanges(result);
        } catch (err) {
          console.log(err);
          setExchanges([]);
        }
      };
      fetch();
    } else {
      setExchanges([]);
    }
  }, [searchText]);

  const onRowClickHandler = (item: IExchangeQueryModel) => {
    setSeachText("");
    setSelectedExchange(item);
    setIsDialogVisible(false);
  };

  const style: CSSProperties = { width: targetRef.current?.clientWidth ?? 200 };

  return (
    <>
      <div className="flex flex-row h-10" ref={targetRef}>
        <div className="relative grow">
          <InputText
            className={classNames(
              inputClass,
              "absolute  w-full h-10 left-0 top-0"
            )}
            value={searchText}
            onChange={(e) => setSeachText(e.target.value)}
          />
          {selectedExchange && (
            <div className="absolute left-2 top-2">{`${selectedExchange.mic} - ${selectedExchange.marketNameInstitutionDescription}`}</div>
          )}
        </div>
        <button
          className="grow-0 bg-green w-8 text-nav content-center"
          onClick={(_) => setIsDialogVisible((prev) => !prev)}
        >
          <EllipsisVerticalIcon className="size-6 mx-1" />
        </button>
      </div>
      <OverlayPanel ref={opRef} closeOnEscape>
        <div
          className="flex w-full border border-green bg-zinc-600"
          style={style}
        >
          <DataTable
            value={exchanges}
            className="w-full"
            onRowClick={(e) => onRowClickHandler(exchanges[e.index])}
            rowClassName={(e) => "hover:bg-highlight dark:hover:bg-highlight"}
          >
            <Column field="code" header="Code" />
            <Column field="marketNameInstitutionDescription" header="Name" />
            <Column field="countryCode" header="Country" />
            <Column field="currencyCode" header="Currency" />
          </DataTable>
        </div>
      </OverlayPanel>
    </>
  );
};
