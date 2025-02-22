import React from "react";
import { Card } from "primereact/card";
import { PortfolioAddEditForm } from "./PortfolioAddEditForm";
import { useParams } from "react-router";
import { PageHeader } from "../../Controls/PageHeader";
import { usePortfolio, useMenuItem } from "../../Hooks";

export const EditPortfolio: React.FC = () => {
  const { portfolioId } = useParams();

  const portfolio = usePortfolio(portfolioId);

  useMenuItem(
    [
      {
        id: `portfolios/${portfolioId}`,
        link: `/portfolios/${portfolioId}`,
        text: portfolio?.name ?? "",
        visible: false,
      },
      {
        id: `portfolios/${portfolioId}/edit`,
        link: `/portfolios/${portfolioId}/edit`,
        text: "Edit",
        visible: false,
      },
    ],
    [portfolio]
  );

  return (
    <div className="flex flex-col">
      <PageHeader title="Edit Portfolio" className="py-5" />
      <Card className="min-w-[500px] w-1/2 ">
        <PortfolioAddEditForm editMode={true} portfolioId={portfolioId} />
      </Card>
    </div>
  );
};
