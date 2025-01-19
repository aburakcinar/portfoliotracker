import React, { useEffect } from "react";
import { IAssetSummaryModel } from "../../Api/AssetApi";
import { fetchSummaryAssets } from "../../Store/AssetsSlice";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";

export default function AssetsSummary() {
  const dispatch = useAppDispatch();
  const summary = useAppSelector((x) => x.assets.summary);

  useEffect(() => {
    dispatch(fetchSummaryAssets());
  }, []);

  const itemClickHandler = (item: IAssetSummaryModel) => {};

  return (
    <div className="pt-16 h-auto">
      <div className="flex w-full justify-center ">
        <div className="w-1/2 min-w-[600px] flex flex-col">
          <h2 className="text-green pb-8 text-5xl">Assets</h2>
          <div className="flex bg-highlight rounded-t-md mt-2 w-full dark:border-b-2 dark:border-nav">
            <div className="grow p-4">Asset Type</div>
            <div className="w-[130px] p-4">Count</div>
          </div>
          {summary.map((item) => {
            return (
              <div
                key={item.assetTypeId}
                className="flex bg-nav last:rounded-b-md w-full hover:bg-highlight"
                onClick={(_) => itemClickHandler(item)}
              >
                <div className="grow p-4">{item.assetType}</div>
                <div className="w-[100px] p-4">{item.count}</div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}
