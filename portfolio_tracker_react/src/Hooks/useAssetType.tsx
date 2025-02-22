import React from "react";
import { IAssetSummaryModel } from "../Api/Asset.api";
import { useAppSelector } from "../Store/RootState";

export const useAssetTypeByName = (
  assetTypeName: string | null | undefined
): IAssetSummaryModel | null => {
  const found = useAppSelector((x) =>
    x.assets.summary.find((p) => p.assetType.toLowerCase() === assetTypeName)
  );

  return found ?? null;
};
