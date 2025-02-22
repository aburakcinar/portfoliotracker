import React from "react";
import { usePortfolio } from "../../Hooks";

export interface IReportSellHoldingFormProps {
  portfolioId: string;
  assetId?: string;
  onChange?: () => void;
  className?: string;
  title?: string;
}

export const ReportSellHoldingForm: React.FC<IReportSellHoldingFormProps> = (
  props
) => {
  const { portfolioId, assetId, onChange } = props;

  const portfolio = usePortfolio(portfolioId);

  return <div></div>;
};
