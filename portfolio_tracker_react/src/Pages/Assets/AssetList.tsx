import React, { useState } from "react";
import {
  AssetTypes,
  IAssetModel,
  IAssetSummaryModel,
} from "../../Api/Asset.api";
import { PageHeader } from "../../Controls/PageHeader";
import { useLocation, useNavigate, useParams } from "react-router";
import { useMenuItem, useAssetList, useAssetTypeByName } from "../../Hooks";
import { IMenuItem } from "../../Store";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { PencilSquareIcon, PlusCircleIcon } from "@heroicons/react/24/outline";
import { Card } from "primereact/card";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { InputText } from "primereact/inputtext";
import { storeSearchParameters } from "../../Store/Assets.slice";

interface IAssetListProps {
  assetType: IAssetSummaryModel;
}

export const AssetList: React.FC<IAssetListProps> = (props) => {
  const { assetType } = props;
  const location = useLocation();
  const navigate = useNavigate();
  const searchParameters = useAppSelector((x) => x.assets.searchParameters);
  const dispatch = useAppDispatch();

  const onSearchTextChangedHandler = (searchText: string) => {
    dispatch(
      storeSearchParameters({
        ...searchParameters,
        searchText,
      })
    );
  };

  useMenuItem(
    {
      id: location.pathname,
      link: location.pathname,
      text: assetType.title,
    } satisfies IMenuItem,
    [assetType]
  );

  const assets = useAssetList(assetType.assetTypeId, searchParameters);

  const onNewStockHandler = () => {
    navigate(`/assets/${assetType.assetType.toLowerCase()}/new`);
  };

  const grigColumnBodyTemplate = (item: IAssetModel) => {
    const onClickHandler = () => {
      const tempMenuItem: IMenuItem = {
        id: `${location.pathname}/${item.id}`,
        text: item.name,
        link: `${location.pathname}/${item.id}`,
      };
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
      <PageHeader title={assetType.title} />
      <Card className="w-full flex flex-col mb-4">
        <label className=" py-1 text-xs dark:text-white text-black">
          Search
        </label>
        <div className="flex gap-2">
          <InputText
            className="grow"
            value={searchParameters.searchText}
            onChange={(e) => onSearchTextChangedHandler(e.target.value)}
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
      <Card className="w-full">
        <DataTable value={assets}>
          <Column field="tickerSymbol" header="Ticker" />
          <Column field="exchangeCode" header="Exchange" />
          <Column field="name" header="Name" />
          <Column field="currencyCode" header="Currency" />
          <Column body={grigColumnBodyTemplate} />
        </DataTable>
      </Card>
    </div>
  );
};

export const AssetListPage: React.FC = () => {
  const { assetTypeName } = useParams();
  const foundAssetType = useAssetTypeByName(assetTypeName);

  if (!foundAssetType) {
    return <span>Not Found</span>;
  }

  return <AssetList assetType={foundAssetType} />;
};
