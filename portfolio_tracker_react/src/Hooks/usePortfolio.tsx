import React, { DependencyList, useEffect, useState } from "react";
import { IPortfolioModel } from "../Store";
import {
  getPortfolioApi,
  getPortfolioTotalApi,
  IPortfolioTotalPositionResultModel,
} from "../Api/Portfolio.api";

export const usePortfolio = (
  portfolioId: string | null | undefined
): IPortfolioModel | null => {
  const [selected, setSelected] = useState<IPortfolioModel | null>(null);

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId) {
        const result = await getPortfolioApi(portfolioId);

        setSelected(result);
      }
    };

    fetch();
  }, [portfolioId]);

  return selected;
};

export const usePortfolioTotalPosition = (
  portfolioId: string | undefined | null,
  dependencies: DependencyList
): IPortfolioTotalPositionResultModel | null => {
  const [data, setData] = useState<IPortfolioTotalPositionResultModel | null>(
    null
  );

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId) {
        const result = await getPortfolioTotalApi(portfolioId);

        setData(result);
      }
    };

    fetch();
  }, [portfolioId, ...dependencies]);

  return data;
};
