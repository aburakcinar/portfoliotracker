import { DependencyList, useEffect, useState } from "react";
import {
  fetchHoldingAssetTransactionsApi,
  fetchHoldingDetailApi,
  IHoldingAssetTransactionModel,
  IHoldingDetailModel,
} from "../Api/HoldingsApi";

export const useHoldingDetail = (
  portfolioId: string | null | undefined,
  assetId: string | null | undefined,
  dependencies: DependencyList
): IHoldingDetailModel[] => {
  const [data, setData] = useState<IHoldingDetailModel[]>([]);

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId && assetId) {
        const result = await fetchHoldingDetailApi(portfolioId, assetId);

        setData(result);
      }
    };

    fetch();
  }, [portfolioId, assetId, ...dependencies]);

  return data;
};

export const useHoldingTransactions = (
  portfolioId: string | null | undefined,
  assetId: string | null | undefined,
  dependencies: DependencyList
): IHoldingAssetTransactionModel[] => {
  const [data, setData] = useState<IHoldingAssetTransactionModel[]>([]);

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId && assetId) {
        const result = await fetchHoldingAssetTransactionsApi(
          portfolioId,
          assetId
        );

        setData(result);
      }
    };

    fetch();
  }, [portfolioId, assetId, ...dependencies]);

  return data;
};
