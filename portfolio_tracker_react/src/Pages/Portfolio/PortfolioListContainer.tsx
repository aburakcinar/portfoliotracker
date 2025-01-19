import React from "react";
import { useAppSelector } from "../../Store/RootState";
import { IPortfolioModel } from "../../Store";

export interface IPortfolioListContainerProps {
  onItemClick?: (item: IPortfolioModel) => void;
}

export const PortfolioListContainer: React.FC<IPortfolioListContainerProps> = (
  props
) => {
  const portfolios = useAppSelector((x) => x.portfolios.portfolios);

  const onClickHandler = (item: IPortfolioModel) => {
    const { onItemClick } = props;

    if (onItemClick) {
      onItemClick(item);
    }
  };

  return (
    <div>
      {portfolios.map((item) => {
        return (
          <div
            onClick={(_) => onClickHandler(item)}
            className="w-full flex dark:bg-nav dark:hover:bg-highlight first:rounded-t-md  last:rounded-b-md border-b-2 border-b-black p-4 text-lg"
          >
            {item.name}
          </div>
        );
      })}
    </div>
  );
};
