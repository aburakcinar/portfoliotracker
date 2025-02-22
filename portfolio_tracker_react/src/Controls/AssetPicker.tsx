import React, { CSSProperties, useEffect, useRef, useState } from "react";

import { InputText } from "primereact/inputtext";
import { OverlayPanel } from "primereact/overlaypanel";
import { classNames } from "primereact/utils";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import {
  AssetTypes,
  getAssetApi,
  IAssetModel,
  searchAssetsApi,
} from "../Api/Asset.api";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

export interface IAssetPickerProps {
  inputClass?: string;
  value: string | null;
  onChange: (e: any) => void;
}

export const AssetPicker: React.FC<IAssetPickerProps> = (props) => {
  const { inputClass, onChange, value } = props;

  const [searchText, setSeachText] = useState<string>("");
  const [isDialogVisible, setIsDialogVisible] = useState<boolean>(true);
  const targetRef = useRef<HTMLDivElement>(null);
  const opRef = useRef<OverlayPanel>(null);
  const [assets, setAssets] = useState<IAssetModel[]>([]);
  const [selectedAsset, setSelectedAsset] = useState<IAssetModel | null>(null);

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
    if (value && value !== "") {
      const fetch = async () => {
        try {
          const result = await getAssetApi(value);

          setSelectedAsset(result);
        } catch (err) {
          console.log(err);
          setSelectedAsset(null);
        }
      };
      fetch();
    }
  }, [value]);

  useEffect(() => {
    onChange(selectedAsset?.id);
  }, [selectedAsset]);

  useEffect(() => {
    if (searchText.length > 0) {
      setSelectedAsset(null);
    }

    setIsDialogVisible((_) => searchText.length > 2);

    if (searchText.length > 2) {
      const fetch = async () => {
        try {
          const result = await searchAssetsApi({
            assetType: null,
            pageIndex: 0,
            pageSize: 10,
            searchText,
          });

          setAssets(result);
        } catch (err) {
          console.log(err);
          setAssets([]);
        }
      };
      fetch();
    } else {
      setAssets([]);
    }
  }, [searchText]);

  const onRowClickHandler = (item: IAssetModel) => {
    setSeachText("");
    setSelectedAsset(item);
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
          {selectedAsset && (
            <div className="absolute left-2 top-2">{selectedAsset.name}</div>
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
            value={assets}
            className="w-full"
            onRowClick={(e) => onRowClickHandler(assets[e.index])}
            rowClassName={(e) => "hover:bg-highlight dark:hover:bg-highlight"}
          >
            <Column field="tickerSymbol" header="Header" />
            <Column field="exchangeCode" header="Exchange" />
            <Column field="name" header="Name" />
            <Column field="price" header="Price" />
          </DataTable>
        </div>
      </OverlayPanel>
    </>
  );
};
