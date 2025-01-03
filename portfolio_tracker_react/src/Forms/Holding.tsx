import React, { useEffect, useState } from "react";
import { IHoldingDetailModel, IPortfolioHoldingModel } from "../Store";
import {
  ChevronDownIcon,
  ChevronUpIcon,
  PlusCircleIcon,
  PencilSquareIcon,
  TrashIcon,
  TagIcon,
  MinusCircleIcon,
  XMarkIcon,
  InboxArrowDownIcon,
} from "@heroicons/react/24/outline";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { AddUpdateHoldingForm } from "./AddUpdateHoldingForn";
import {
  setSelectedReportSellItem,
  setShowReportSellDialog,
  useAppDispatch,
  useAppSelector,
} from "../Store/RootState";
import { deleteHolding, fetchHoldingDetail } from "../Store/Thunks";

import { ConfirmPopup, confirmPopup } from "primereact/confirmpopup";
import { Dialog } from "primereact/dialog";
import { Calendar } from "primereact/calendar";
import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";
import { ReportSellHoldingDialog } from "./ReportSellHoldingDialog";

export interface IHoldingsProps {
  portfolioId: string;
  holding: IPortfolioHoldingModel;
  currencySymbol: string;
  currencyCode: string;
}

export const Holding: React.FC<IHoldingsProps> = (props) => {
  const { portfolioId, holding, currencySymbol, currencyCode } = props;
  const [expanded, setExpanded] = useState<boolean>(false);
  //const [details, setDetails] = useState<IHoldingDetailModel[]>([]);
  const [editing, setEditing] = useState<boolean>(false);
  const [editingHolding, setEditingHolding] =
    useState<IHoldingDetailModel | null>(null);

  const details = useAppSelector(
    (x) => x.portfolios.holdingDetails[`${portfolioId}_${holding.stockSymbol}`]
  );
  const dispatch = useAppDispatch();

  useEffect(() => {
    const { stockSymbol } = holding;

    dispatch(fetchHoldingDetail({ portfolioId, stockSymbol }));
  }, [expanded]);

  const formatCurrency = (value: number) => {
    return value.toLocaleString("tr-TR", {
      style: "currency",
      currency: currencyCode,
    });
  };

  const priceBodyTemplate = (product: IHoldingDetailModel) => {
    return formatCurrency(product.price);
  };

  const dateBodyTemplate = (rowData: IHoldingDetailModel) => {
    return formatDate(new Date(rowData.executeDate));
  };

  const formatDate = (value: Date) => {
    return value.toLocaleDateString("en-US", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    });
  };

  const onEditHandler = (item: IHoldingDetailModel) => {
    setEditingHolding(item);
    setEditing(true);
  };

  const onConfirmDeleteHandler = (
    event: React.MouseEvent<HTMLElement>,
    item: IHoldingDetailModel
  ) => {
    const accept = () => {
      const { stockSymbol } = holding;
      const { id: holdingId } = item;

      dispatch(
        deleteHolding({
          portfolioId,
          stockSymbol,
          holdingId,
        })
      );
    };

    if (event.currentTarget) {
      confirmPopup({
        target: event.currentTarget,
        message: "Do you want to delete this holding?",
        icon: "pi pi-info-circle",
        defaultFocus: "reject",
        acceptClassName: "p-button-danger",
        accept,
      });
    }
  };

  const onShowReportSellDialog = (item: IHoldingDetailModel) => {
    dispatch(setShowReportSellDialog(true));
    dispatch(setSelectedReportSellItem(item));
  };

  const contextMenuTemplate = (item: IHoldingDetailModel) => {
    return (
      <div className="grid grid-cols-3 gap-1">
        <button title="Edit" onClick={(_) => onEditHandler(item)}>
          <PencilSquareIcon className="size-5 " />
        </button>

        <button
          title="Report Sell"
          onClick={(_) => onShowReportSellDialog(item)}
        >
          <TagIcon className="size-5" />
        </button>
        <ConfirmPopup />
        <button onClick={(e) => onConfirmDeleteHandler(e, item)} title="Delete">
          <TrashIcon className="size-5 text-orange-600" />
        </button>
      </div>
    );
  };

  const closeEditingHandler = () => {
    setEditing(false);
    setEditingHolding(null);
  };

  return (
    <>
      <div className="flex bg-nav last:rounded-b-md w-full">
        <div className="grow p-4">{holding.stockSymbol}</div>
        <div className="w-[100px] p-4">{holding.quantity}</div>
        <div className="w-[110px] p-4">
          {`${currencySymbol} ${holding.averagePrice.toFixed(2)}`}
        </div>
        <div className="w-[130px] p-4">
          {`${currencySymbol} ${(
            holding.averagePrice * holding.quantity
          ).toFixed(2)}`}
        </div>
        <div className="w-[20px] p-4 grid  place-content-center">
          <PlusCircleIcon
            className="size-5"
            onClick={(_) => {
              setEditing((prev) => !prev);
              setEditingHolding(null);
              setExpanded(true);
            }}
          />
        </div>
        <div className="w-[20px] p-4 grid place-content-center">
          <div>
            {!expanded && (
              <ChevronDownIcon
                className="size-5"
                onClick={(_) => setExpanded(true)}
              />
            )}
            {expanded && (
              <ChevronUpIcon
                className="size-5"
                onClick={(_) => setExpanded(false)}
              />
            )}
          </div>
        </div>
      </div>
      {expanded && (
        <div className="flex w-full flex-col bg-nav p-2">
          {editing && (
            <AddUpdateHoldingForm
              currencyCode={currencyCode}
              currencySymbol={currencySymbol}
              holding={editingHolding}
              portfolioId={portfolioId}
              stockSymbol={holding.stockSymbol}
              onClose={closeEditingHandler}
            />
          )}
          <DataTable value={details} className="w-full">
            <Column field="quantity" header="Quantity"></Column>
            <Column
              field="price"
              header="Price"
              body={priceBodyTemplate}
            ></Column>
            <Column field="expenses" header="Expenses"></Column>
            <Column
              field="executeDate"
              header="Execute Date"
              body={dateBodyTemplate}
            ></Column>
            <Column body={contextMenuTemplate} bodyClassName="px-0"></Column>
          </DataTable>
          <ReportSellHoldingDialog
            portfolioId={portfolioId}
            stockSymbol={holding.stockSymbol}
            currencyCode={currencyCode}
            currencySymbol={currencySymbol}
          />
        </div>
      )}
    </>
  );
};
