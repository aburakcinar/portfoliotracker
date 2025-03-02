import { DependencyList, useEffect, useState } from "react";
import {
  fetchHoldingAssetTransactionsApi,
  fetchHoldingDetailApi,
  IHoldingAssetTransactionModel,
  IHoldingDetailModel,
} from "../Api/HoldingsApi";
import { getHoldingAssetSummaryApi } from "../Api/Asset.api";
import { IHoldingAssetSummaryModel } from "../Api/Asset.api";

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

export const useHoldingAssetSummary = (
  portfolioId: string | null | undefined,
  assetId: string | null | undefined,
  dependencies: DependencyList
): IHoldingAssetSummaryModel | null => {
  const [data, setData] = useState<IHoldingAssetSummaryModel | null>(null);

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId && assetId) {
        const result = await getHoldingAssetSummaryApi(portfolioId, assetId);

        setData(result);
      }
    };

    fetch();
  }, [portfolioId, assetId, ...dependencies]);

  return data;
};
