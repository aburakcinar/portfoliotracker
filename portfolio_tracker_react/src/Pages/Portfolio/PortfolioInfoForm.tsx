import { Card } from "primereact/card";
import React from "react";
import { usePortfolio } from "../../Hooks";
import { classNames } from "primereact/utils";

export interface IPortfolioInfoFormProps {
  portfolioId: string;
  className?: string;
}

export const PortfolioInfoForm: React.FC<IPortfolioInfoFormProps> = (props) => {
  const { portfolioId } = props;

  const portfolio = usePortfolio(portfolioId);

  return (
    <Card
      title="Portfolio"
      className={classNames("flex flex-col", props.className)}
    >
      <div className="flex flex-row">
        <div className="grow text-green text-2xl">{portfolio?.name}</div>
        <div className="grow-0 m-2 text-xl">{portfolio?.currencyName}</div>
        <div className="grow-0 m-2 text-xl">{portfolio?.currencySymbol}</div>
      </div>
    </Card>
  );
};
