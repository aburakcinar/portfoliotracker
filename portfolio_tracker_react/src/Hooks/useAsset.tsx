import React, { DependencyList, useEffect, useState } from "react";
import { getAssetApi, IAssetModel } from "../Api/Asset.api";

export const useAsset = (
  assetId: string | null | undefined,
  dependencies: DependencyList
): IAssetModel | null => {
  const [asset, setAsset] = useState<IAssetModel | null>(null);

  useEffect(() => {
    const fetch = async () => {
      if (assetId) {
        const result = await getAssetApi(assetId);
        setAsset(result);
      }
    };

    fetch();
  }, [assetId, ...dependencies]);

  return asset;
};
