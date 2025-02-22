import { Card } from "primereact/card";
import React from "react";
import { PortfolioAddEditForm } from "./PortfolioAddEditForm";

export const NewPortfolio: React.FC = () => {
  return (
    <Card title="New Portfolio">
      <PortfolioAddEditForm editMode={false} />
    </Card>
  );
};
