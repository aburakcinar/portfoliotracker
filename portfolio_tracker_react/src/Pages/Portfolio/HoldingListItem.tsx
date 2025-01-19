import React, { CSSProperties, useState } from "react";
import { IHoldingAggregateModel } from "../../Api/HoldingsApi";
import {
  EllipsisVerticalIcon,
  ArrowUpTrayIcon,
  ListBulletIcon,
} from "@heroicons/react/24/outline";
import { PortfolioListContainer } from "./PortfolioListContainer";

export interface IHoldingListItemProps {
  item: IHoldingAggregateModel;
}

const MOVE_TO_PORTFOLIO = "MOVE_TO_PORTFOLIO";

export const HoldingListItem: React.FC<IHoldingListItemProps> = (props) => {
  const [showMenu, setShowMenu] = useState<boolean>(false);
  const [targetContextMenuType, setTargetContextMenuType] =
    useState<string>("");

  const { item } = props;
  const { color } = item;

  const iconStyle: CSSProperties = {
    backgroundColor: color,
  };

  const toggleMenu = () => {
    setShowMenu((prev) => !prev);
  };

  const onOpenMoveFormHandler = () => {
    setTargetContextMenuType(MOVE_TO_PORTFOLIO);
  };

  return (
    <div className="flex flex-col">
      <div
        key={item.assetId}
        className="flex flex-row gap-2 pb-2 border-b-2 border-b-slate-700 mb-2"
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

        {showMenu && (
          <div className="grow-0 py-1 flex flex-row">
            {/* context menu */}
            <button
              className="flex flex-row bg-green hover:bg-green/60 p-2 text-black/80 m-1 mr-0"
              title="Move to another portfolio"
              onClick={onOpenMoveFormHandler}
            >
              <ArrowUpTrayIcon className="size-5" />
              <div>Move</div>
            </button>
            <button
              className="flex flex-row bg-green hover:bg-green/60 p-2 text-black/80 m-1"
              title="List transactions"
            >
              <ListBulletIcon className="size-5" />
              <div>Transactions</div>
            </button>
          </div>
        )}
        <div className="grow-0 py-1">
          <button onClick={toggleMenu}>
            <EllipsisVerticalIcon className="w-5 h-10 py-2" />
          </button>
        </div>
      </div>
      {targetContextMenuType === MOVE_TO_PORTFOLIO && (
        <PortfolioListContainer />
      )}
    </div>
  );
};
