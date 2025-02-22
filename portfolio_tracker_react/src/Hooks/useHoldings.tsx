import React, { DependencyList, useEffect, useState } from "react";
import {
  fetchHoldingsByPortfolioIdApi,
  IHoldingAggregateModel,
} from "../Api/HoldingsApi";
import { getRandomColor } from "../Tools/getRandomColor";

export const useHoldings = (
  portfolioId: string | null | undefined,
  dependencies: DependencyList
): IHoldingAggregateModel[] => {
  const [holdings, setHoldings] = useState<IHoldingAggregateModel[]>([]);

  useEffect(() => {
    const fetch = async () => {
      if (portfolioId) {
        const resultHoldings = await fetchHoldingsByPortfolioIdApi(portfolioId);
        const resultHoldingWithColor = [
          ...resultHoldings.map((x) => {
            return {
              ...x,
              color: getRandomColor(),
            };
          }),
        ];
        setHoldings(resultHoldingWithColor);
      }
    };
    fetch();
  }, [portfolioId, ...dependencies]);

  return holdings;
};
