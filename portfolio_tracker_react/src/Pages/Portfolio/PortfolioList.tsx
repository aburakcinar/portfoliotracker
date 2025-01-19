import React, { useEffect } from "react";
import {
  useAppSelector,
  useAppDispatch,
  setShowNewPortfolioForm,
} from "../../Store/RootState";
import { fetchPortfolios } from "../../Store/Thunks";
import { PlusIcon } from "@heroicons/react/24/outline";
import { NewPortfolio } from "./NewPortfolio";
import { useNavigate } from "react-router";
import { PortfolioListContainer } from "./PortfolioListContainer";
import { IPortfolioModel } from "../../Store";

export const PortfolioList: React.FC = () => {
  const navigate = useNavigate();
  const showNewPortfolioForm = useAppSelector(
    (x) => x.portfolios.showNewPortfolioForm
  );

  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fetchPortfolios());
  }, []);

  const showCreatePortfolioForm = () => {
    dispatch(setShowNewPortfolioForm(true));
  };

  const onPortfolioItemClickHandler = (item: IPortfolioModel) => {
    navigate(`/portfolios/${item.id}`);
  };

  return (
    <div className="pt-16 h-auto flex  w-full justify-center">
      <div className="flex flex-col min-w-[500px] w-1/2  mt-5  rounded-md  ">
        <div className="flex mb-2">
          <h2 className="grow text-green text-3xl">Portfolios </h2>

          {!showNewPortfolioForm && (
            <button
              className="grow-0 bg-highlight  rounded-md  p-1"
              onClick={showCreatePortfolioForm}
            >
              <PlusIcon
                className="size-8  text-green"
                title="Create New Portfolio"
              />
            </button>
          )}
        </div>
        {showNewPortfolioForm && <NewPortfolio />}
        <PortfolioListContainer onItemClick={onPortfolioItemClickHandler} />
      </div>
    </div>
  );
};
