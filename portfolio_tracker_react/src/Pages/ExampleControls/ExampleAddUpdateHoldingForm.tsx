import React from "react";
import { Card } from "primereact/card";
import {
  AddUpdateHoldingForm,
  IAddUpdateHoldingFormProps,
} from "../../Forms/AddUpdateHoldingForn";

export const ExampleAddUpdateHoldingForm: React.FC = () => {
  const addUpdateHoldingProps: IAddUpdateHoldingFormProps = {
    currencyCode: "TRY",
    currencySymbol: "T",
    holding: null,
    portfolioId: "ExamplePortfolioId",
    stockSymbol: "TUPRS",
  };
  return (
    <>
      <h3 className="text-2xl text-green m-10 my-4">
        Add Or Update Holding Form
      </h3>
      <Card className="m-10">
        <AddUpdateHoldingForm {...addUpdateHoldingProps} />
      </Card>
    </>
  );
};
