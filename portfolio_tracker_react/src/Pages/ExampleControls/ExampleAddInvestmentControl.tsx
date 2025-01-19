import React from "react";
import { Card } from "primereact/card";
import { AddInvestmentControl } from "../../Popups/AddInvestmentControl";

export const ExampleAddInvestmentControl: React.FC = () => {
  return (
    <>
      <h3 className="text-2xl text-green m-10 my-4">Add New Investment Form</h3>
      <Card className="m-10">
        <AddInvestmentControl currency="TRY" portfolioId="examplePortfolio" />
      </Card>
    </>
  );
};
