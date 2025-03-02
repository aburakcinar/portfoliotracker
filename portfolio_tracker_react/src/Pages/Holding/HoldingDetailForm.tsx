import React, { useState } from "react";
import { PageHeader } from "../../Controls/PageHeader";
import { useParams } from "react-router";
import { useAsset, usePortfolio } from "../../Hooks";
import { Card } from "primereact/card";
import {
  useHoldingAssetSummary,
  useHoldingDetail,
  useHoldingTransactions,
} from "../../Hooks/useHoldingDetail";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { AddHoldingForm } from "./AddHoldingForm";
import { PortfolioInfoForm } from "../Portfolio/PortfolioInfoForm";
import { Button } from "primereact/button";
import { Sidebar } from "primereact/sidebar";
import { IHoldingAssetTransactionModel } from "../../Api/HoldingsApi";
import { classNames } from "primereact/utils";
import { ReportSellHoldingForm } from "./ReportSellHoldingForm";
import { HoldingPositionDetailControl } from "./HoldingPositionDetailControl";

export interface IHoldingDetailFormProps {
  portfolioId: string;
  assetId: string;
}

export const HoldingDetailForm: React.FC<IHoldingDetailFormProps> = (props) => {
  const { portfolioId, assetId } = props;

  const [isVisibleAddHoldingForm, setIsVisibleAddHoldingForm] =
    useState<boolean>(false);
  const [isVisibleSellHoldingForm, setIsVisibleSellHoldingForm] =
    useState<boolean>(false);

  const [iteration, setIteration] = useState<number>(0);

  const asset = useAsset(assetId, [assetId]);
  const portfolio = usePortfolio(portfolioId);
  const holdingAssetSummary = useHoldingAssetSummary(portfolio?.id, asset?.id, [
    portfolioId,
    assetId,
    iteration,
  ]);

  const holdingDetails = useHoldingDetail(portfolio?.id, asset?.id, [
    iteration,
  ]);
  const holdingTransactions = useHoldingTransactions(portfolio?.id, asset?.id, [
    iteration,
  ]);

  const onHoldingAddedHandler = () => {
    setIteration((prev) => prev + 1);
    setIsVisibleAddHoldingForm(false);
  };

  const onHoldingSoldHandler = () => {
    setIteration((prev) => prev + 1);
    setIsVisibleSellHoldingForm(false);
  };

  return (
    <div className="flex flex-col w-1/2 mt-5 ">
      <div className="flex flex-row w-full px-2 gap-1">
        <PageHeader title={asset?.name ?? ""} className=" grow" />
        <Button
          className="grow-0 dark:bg-green dark:hover:bg-green/70  dark:text-black h-12"
          onClick={(_) => setIsVisibleAddHoldingForm(true)}
        >
          Report Buy
        </Button>
        <Button
          className="grow-0 dark:bg-green  dark:hover:bg-green/70  dark:text-black h-12"
          onClick={(_) => setIsVisibleSellHoldingForm(true)}
        >
          Report Sell
        </Button>
      </div>
      <div className="flex flex-row">
        <span className="dark:text-white/60 mr-1">ISIN</span>
        <span className="dark:text-white/80 dark:hover:text-white mr-2">
          {asset?.isin}
        </span>
        <span className="dark:text-white/60 mr-1">WKN</span>
        <span className="dark:text-white/80 dark:hover:text-white">
          {asset?.wkn}
        </span>
      </div>

      <div className="mt-5">
        <span className="dark:text-white/60 mr-1">Last Price</span>
        <span className="text-4xl">
          {asset?.currencySymbol}
          {asset?.price}
        </span>
      </div>

      <PortfolioInfoForm portfolioId={portfolioId} className="mt-5" />

      {portfolio?.id && asset?.id && (
        <HoldingPositionDetailControl
          portfolioId={portfolio.id}
          assetId={asset.id}
          iteration={iteration}
        />
      )}

      {/* <Card title="Holdings">
            <DataTable value={holdingDetails}>
              <Column field="executeDate" header="Date" />
              <Column field="quantity" header="Quantity" />
              <Column field="price" header="Price" />
              <Column field="total" header="Total" />
              <Column field="currencyCode" header="Currency" />
            </DataTable>
          </Card> */}

      <Card title="Transactions" className="mt-5">
        <DataTable value={holdingTransactions}>
          <Column field="executeDate" header="Date" />
          <Column field="description" header="Desc" />
          <Column
            field="quantity"
            header="Quantity"
            body={(item: IHoldingAssetTransactionModel) => `${item.quantity}`}
          />
          <Column
            field="price"
            header="Price"
            body={(item: IHoldingAssetTransactionModel) =>
              `${item.currencySymbol} ${item.price}`
            }
          />
          <Column
            field="total"
            header="Total"
            body={(item: IHoldingAssetTransactionModel) => (
              <div
                className={classNames(
                  { "text-green": item.total >= 0 },
                  { "text-red-600": item.total < 0 }
                )}
              >
                {`${item.currencySymbol} ${item.total}`}
              </div>
            )}
          />
        </DataTable>
      </Card>
      {/* <AddHoldingForm
          title="Add Holding"
          portfolioId={portfolioId}
          assetId={asset?.id}
          onChange={onHoldingAddedHandler}
          className="col-span-2"
        /> */}
      <Sidebar
        visible={isVisibleAddHoldingForm}
        position="right"
        onHide={() => setIsVisibleAddHoldingForm(false)}
        className="w-[500px]"
      >
        <AddHoldingForm
          title="Add Holding"
          portfolioId={portfolioId}
          assetId={asset?.id}
          onChange={onHoldingAddedHandler}
          className="col-span-2"
        />
      </Sidebar>

      <Sidebar
        visible={isVisibleSellHoldingForm}
        position="right"
        onHide={() => setIsVisibleSellHoldingForm(false)}
        className="w-[500px]"
      >
        <ReportSellHoldingForm
          title="Sell Holding"
          portfolioId={portfolioId}
          assetId={asset?.id}
          onChange={onHoldingSoldHandler}
          className="col-span-2"
        />
      </Sidebar>
    </div>
  );
};

export const HoldingDetailPage: React.FC = () => {
  const { portfolioId, assetId } = useParams();

  if (!portfolioId || !assetId) {
    return null;
  }

  return <HoldingDetailForm portfolioId={portfolioId} assetId={assetId} />;
};
