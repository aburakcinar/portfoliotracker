import React, { useEffect } from "react";
import { IAssetSummaryModel } from "../../Api/Asset.api";
import { fetchSummaryAssets } from "../../Store/Assets.slice";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { PageHeader } from "../../Controls/PageHeader";
import { useNavigate } from "react-router";

export function AssetsSummary() {
  const dispatch = useAppDispatch();
  const summary = useAppSelector((x) => x.assets.summary);
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(fetchSummaryAssets());
  }, []);

  const itemClickHandler = (item: IAssetSummaryModel) => {
    navigate(`/assets/${item.assetType.toLowerCase()}`);
  };

  return (
    <div className="pt-16 h-auto">
      <div className="flex w-full justify-center ">
        <div className="w-1/2 min-w-[600px] flex flex-col">
          <PageHeader title="Assets" />
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
