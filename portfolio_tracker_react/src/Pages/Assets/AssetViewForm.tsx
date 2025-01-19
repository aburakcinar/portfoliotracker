import { Card } from "primereact/card";
import React, { useEffect, useState } from "react";
import { useLocation, useParams } from "react-router";
import { getAssetApi, IAssetModel } from "../../Api/AssetApi";
import { useAppDispatch } from "../../Store/RootState";
import { addMenuItem, IMenuItem } from "../../Store";

export const AssetViewForm: React.FC = () => {
  const location = useLocation();
  const dispatch = useAppDispatch();
  const { id } = useParams();
  const [asset, setAsset] = useState<IAssetModel | null>(null);

  useEffect(() => {
    const fetch = async () => {
      if (id) {
        const result = await getAssetApi(id);

        setAsset(result);

        if (result) {
          const { id, name } = result;
          const tempMenuItem: IMenuItem = {
            id: `${location.pathname}/${id}`,
            text: name,
            link: `${location.pathname}/${id}`,
          };
          dispatch(addMenuItem(tempMenuItem));
        }
      }
    };

    fetch();
  }, []);

  if (!asset) {
    return null;
  }

  return (
    <div className="grid grid-cols-12 gap-4 w-full mx-4">
      <h3 className="text-3xl text-green pt-2 col-span-12 justify-start">
        {asset.name} ({asset.tickerSymbol} : {asset.exchangeCode})
      </h3>
      <div className="flex flex-row text-6xl text-white/80 pb-2 col-span-12 justify-start">
        <div>{asset.price.toFixed(2)}</div>
        <div className="text-2xl ml-1">{asset.currencySymbol}</div>
      </div>
      <Card className="col-span-8 row-span-2 text-black dark:text-white/60">
        Graph
      </Card>
      <Card className="col-span-4 text-black dark:text-white/60">
        <div className="text-black dark:text-white text-xl pb-5">Exchange</div>

        <div>{asset.exchangeCode}</div>
        <div>{asset.exchangeName}</div>
        <div>{asset.exchangeCountryCode}</div>
      </Card>
      <Card className="col-span-4 grid grid-cols-2 text-black dark:text-white/60">
        <label>ISIN</label>
        <div>{asset.isin}</div>
        <label>WKN</label>
        <div>{asset.wkn}</div>
      </Card>
      <Card className="col-span-8 text-black dark:text-white/60 flex flex-col">
        <label className=" text-black dark:text-white text-xl">About</label>
        <div className="mt-2">{asset.description}</div>
      </Card>
    </div>
  );
};
