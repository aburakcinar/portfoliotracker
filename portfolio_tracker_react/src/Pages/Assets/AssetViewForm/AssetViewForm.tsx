import { Card } from "primereact/card";
import React, { useState } from "react";
import { useLocation, useParams } from "react-router";
import { useAsset, useMenuItem } from "../../../Hooks";
import { AssetViewDescriptionControl } from "./AssetViewDescriptionControl";
import { DisplayItem } from "../../../Controls/DisplayItem";
import { updateAssetFieldApi } from "../../../Api/Asset.api";

export const AssetViewForm: React.FC = () => {
  const location = useLocation();
  const { id } = useParams();
  const [iteration, setIteration] = useState<number>(0);

  const asset = useAsset(id, []);
  useMenuItem(
    {
      id: location.pathname,
      text: asset?.name ?? "",
      link: location.pathname,
    },
    [asset?.name, iteration]
  );

  console.log("AssetViewForm hit!");

  const onIsinEditHandler = (value: string) => {
    const update = async () => {
      if (asset) {
        var result = await updateAssetFieldApi(asset.id, "Isin", value);

        if (result) {
          setIteration((prev) => prev + 1);
        }
      }
    };
    update();
  };

  const onWknEditHandler = (value: string) => {
    const update = async () => {
      if (asset) {
        var result = await updateAssetFieldApi(asset.id, "Wkn", value);

        if (result) {
          setIteration((prev) => prev + 1);
        }
      }
    };
    update();
  };

  const onWebSiteEditHandler = (value: string) => {
    const update = async () => {
      if (asset) {
        var result = await updateAssetFieldApi(asset.id, "WebSite", value);

        if (result) {
          setIteration((prev) => prev + 1);
        }
      }
    };
    update();
  };

  if (!asset) {
    return null;
  }

  return (
    <div className="grid grid-cols-12 gap-4 w-full mx-4">
      <h3 className="text-3xl text-green pt-2 col-span-12 justify-start">
        {asset.name} ({`${asset.exchangeCode}#${asset.tickerSymbol}`} )
      </h3>
      <div className="flex flex-row text-6xl text-white/80 pb-2 col-span-12 justify-start">
        <div>{asset.price.toFixed(2)}</div>
        <div className="text-2xl ml-1">{asset.currencySymbol}</div>
      </div>
      <Card
        title="Graph"
        className="col-span-8 row-span-2 text-black dark:text-white/60"
      ></Card>
      <Card
        title="Exchange"
        className="col-span-4 text-black dark:text-white/60"
      >
        <div>{asset.exchangeCode}</div>
        <div>{asset.exchangeName}</div>
        <div>{asset.exchangeCountryCode}</div>
      </Card>
      <Card
        title="Info"
        className="col-span-4 grid  text-black dark:text-white/60"
      >
        <DisplayItem
          className="py-1"
          labelClassName="dark:text-white/60"
          label="ISIN"
          editable={true}
          onEdit={onIsinEditHandler}
          content={asset.isin}
        />
        <DisplayItem
          className="py-1"
          labelClassName="dark:text-white/60"
          label="WKN"
          editable={true}
          onEdit={onWknEditHandler}
          content={asset.wkn}
        />
      </Card>

      <AssetViewDescriptionControl
        asset={asset}
        onUpdate={() => setIteration((prev) => prev + 1)}
      />

      <Card
        title="Other info"
        className="col-span-4 grid  text-black dark:text-white/60"
      >
        <DisplayItem
          className="py-1"
          labelClassName="dark:text-white/60"
          label="Web Site"
          editable={true}
          onEdit={onWebSiteEditHandler}
          content={asset.webSite}
        />
      </Card>
    </div>
  );
};
