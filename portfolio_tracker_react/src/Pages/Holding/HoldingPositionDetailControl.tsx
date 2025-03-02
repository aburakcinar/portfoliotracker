import React from "react";
import { Card } from "primereact/card";
import { classNames } from "primereact/utils";
import { IHoldingAssetSummaryModel } from "../../Api/Asset.api";
import { useHoldingAssetSummary } from "../../Hooks/useHoldingDetail";
import useCurrencySymbol from "../../Hooks/useCurrencySymbol";

interface IHoldingPositionDetailControlProps {
  portfolioId: string;
  assetId: string;
  className?: string;
  iteration?: number;
}

export const HoldingPositionDetailControl: React.FC<
  IHoldingPositionDetailControlProps
> = ({ portfolioId, assetId, className, iteration = 0 }) => {
  const holdingAssetSummary = useHoldingAssetSummary(portfolioId, assetId, [
    portfolioId,
    assetId,
    iteration,
  ]);

  const portfolioCurrencySymbol = useCurrencySymbol(
    holdingAssetSummary?.portfolioCurrency ?? ""
  );

  if (!holdingAssetSummary) {
    return null;
  }
  const {
    portfolioCurrency,
    totalProfitLoss,
    realizedProfitLoss,
    unrealizedProfitLoss,
    totalFees,
    totalTaxes,
  } = holdingAssetSummary;

  return (
    <div className={classNames("grid grid-cols-3 gap-4 mt-5", className)}>
      <Card title="Total P/L">
        <div
          className={classNames(
            "text-2xl",
            { "text-green": totalProfitLoss >= 0 },
            { "text-red-600": totalProfitLoss < 0 }
          )}
        >
          {portfolioCurrencySymbol} {totalProfitLoss}
        </div>
      </Card>
      <Card title="Realized P/L">
        <div
          className={classNames(
            "text-2xl",
            { "text-green": realizedProfitLoss >= 0 },
            { "text-red-600": realizedProfitLoss < 0 }
          )}
        >
          {portfolioCurrencySymbol} {realizedProfitLoss}
        </div>
        {totalFees > 0 && (
          <div className="text-sm text-gray-500 mt-2">
            Total Fees: {portfolioCurrency}{" "}
            <span className="text-red-600">{totalFees}</span>
          </div>
        )}
        {totalTaxes > 0 && (
          <div className="text-sm text-gray-500">
            Total Taxes: {portfolioCurrency} {totalTaxes}
          </div>
        )}
      </Card>
      <Card title="Unrealized P/L">
        <div
          className={classNames(
            "text-2xl",
            {
              "text-green": unrealizedProfitLoss >= 0,
            },
            {
              "text-red-600": unrealizedProfitLoss < 0,
            }
          )}
        >
          {portfolioCurrencySymbol} {unrealizedProfitLoss}
        </div>
      </Card>
    </div>
  );
};
