import React from "react";
import { PageHeader } from "../../Controls/PageHeader";

export const ExchangeRatesPage: React.FC = () => {
  return (
    <div className="flex flex-row mt-20 w-full">
      <div className="flex flex-row w-1/2 justify-center">
        <PageHeader title="Exchange Rates" />
      </div>
    </div>
  );
};
