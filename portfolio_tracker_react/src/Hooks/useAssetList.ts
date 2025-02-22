import React, { useEffect, useState } from "react";
import {
  AssetTypes,
  IAssetModel,
  ISearchAssetBaseRequest,
  searchAssetsApi,
} from "../Api/Asset.api";

export const useAssetList = (
  assetType: AssetTypes,
  searchParams: ISearchAssetBaseRequest
): IAssetModel[] => {
  const [assets, setAssets] = useState<IAssetModel[]>([]);

  const { searchText, pageIndex, pageSize } = searchParams;

  useEffect(() => {
    const fetch = async () => {
      const result = await searchAssetsApi({
        ...searchParams,
        assetType,
      });

      setAssets(result);
    };

    fetch();
  }, [assetType, searchText, pageIndex, pageSize]);

  return assets;
};
