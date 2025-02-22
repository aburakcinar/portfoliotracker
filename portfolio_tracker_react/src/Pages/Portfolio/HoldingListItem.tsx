import React, { CSSProperties, useState } from "react";
import { IHoldingAggregateModel } from "../../Api/HoldingsApi";
import {
  EllipsisVerticalIcon,
  ArrowUpTrayIcon,
  ListBulletIcon,
} from "@heroicons/react/24/outline";
import { PortfolioListContainer } from "./PortfolioListContainer";
import { useLocation, useNavigate } from "react-router";

export interface IHoldingListItemProps {
  item: IHoldingAggregateModel;
}

const MOVE_TO_PORTFOLIO = "MOVE_TO_PORTFOLIO";

export const HoldingListItem: React.FC<IHoldingListItemProps> = (props) => {
  const { item } = props;
  const { color } = item;
  const navigate = useNavigate();
  const location = useLocation();

  const iconStyle: CSSProperties = {
    backgroundColor: color,
  };

  const onItemClickHandler = () => {
    navigate(`${location.pathname}/holding/${item.assetId}`);
  };

  return (
    <div
      key={item.assetId}
      className="flex flex-row gap-2 pb-2 border-b-2 border-b-slate-700 mb-2 hover:bg-nav"
      onClick={onItemClickHandler}
    >
      <div className="grow-0 max-w-18 h-full align-middle py-3">
        <div
          style={iconStyle}
          className="w-full h-7 rounded-md border-0 text-xs font-bold text-center align-middle p-1 "
        >
          {item.assetTickerSymbol}
        </div>
      </div>
      <div className="grow text">
        <div className="py-1">{item.assetName}</div>
        <div className="text-xs text-white/60">
          [{item.exchangeCode}:{item.assetTickerSymbol}] {item.totalQuantity}{" "}
          Shares
        </div>
      </div>
      <div className="grow-0 py-1">
        <div className="text-white text-right pb-1">
          {item.currencySymbol}
          {item.totalCost}
        </div>
        <div className="text-white/60 text-xs text-right">
          {item.currencySymbol}
          {item.assetPrice}
        </div>
      </div>
    </div>
  );
};
