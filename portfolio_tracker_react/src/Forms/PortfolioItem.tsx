import { IPortfolioItem } from "../Api/PortfolioApi";
import { AddInvestmentPopup } from "../Popups/AddInvestmentPopup";

interface IPortfolioItemProps {
  portfolio: IPortfolioItem;
}

const PortfolioItem: React.FC<IPortfolioItemProps> = (props) => {
  const { portfolio } = props;
  const { id, name, currencyCode, currencySymbol } = portfolio;

  return (
    <div className="flex flex-col">
      <div className="flex w-full py-2 pt-4">
        <div className="grow">
          <span className="text-green text-xl">{name}</span>
          <span> Portfolio</span>
        </div>
        <div className="grow-0">
          <AddInvestmentPopup currency={currencyCode} portfolioId={id} />
        </div>
      </div>
      <div className="flex bg-highlight rounded-t-md mt-2 w-full">
        <div className="grow p-4">Symbol ({currencyCode})</div>
        <div className="w-[100px] p-4">Amount</div>
        <div className="w-[100px] p-4">Price {currencySymbol}</div>
        <div className="w-[100px] p-4">Total {currencySymbol}</div>
      </div>
    </div>
  );
};

export { type IPortfolioItemProps, PortfolioItem };
