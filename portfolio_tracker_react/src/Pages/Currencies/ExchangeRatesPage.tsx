import React from "react";
import { PageHeader } from "../../Controls/PageHeader";
import { Card } from "primereact/card";

export const ExchangeRatesPage: React.FC = () => {
  return (
    <div className="flex flex-col mt-20 w-full justify-center">
      <div className="flex flex-col w-1/2 ">
        <PageHeader title="Exchange Rates" />

        <Card title="Rates">
          <svg></svg>
        </Card>
      </div>
    </div>
  );
};
