import React, { useEffect, useState } from "react";
import { PlusIcon } from "@heroicons/react/24/outline";
import CreatePortfolioForm from "../Forms/CreatePortfolioForm";
import { IPortfolioItem, listPortfolios } from "../Api/PortfolioApi";
import { PortfolioItem } from "../Forms/PortfolioItem";

export default function Portfolio() {
  const [showNewPortfolioForm, setShowNewPortfolioForm] =
    useState<boolean>(false);

  const [fetchCounter, setFetchCounter] = useState<number>(0);
  const [portfolios, setPortfolios] = useState<IPortfolioItem[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const data = await listPortfolios();

      console.log("portfolios", data);

      setPortfolios(data);
    };

    fetch();
  }, [fetchCounter]);

  const showCreatePortfolioForm = () => {
    setShowNewPortfolioForm(true);
  };

  const onNewPortfolioHandler = () => {
    console.log("new portfolio");
    setShowNewPortfolioForm(false);
    setFetchCounter((val) => val + 1);
  };

  const onPortfolioFormCloseHandler = () => {
    setShowNewPortfolioForm(false);
  };

  return (
    <div className="pt-16">
      <h2 className="text-green pl-8 text-3xl">Portfolio</h2>

      <div className="flex">
        <div className="flex-auto w-32">&nbsp;</div>
        <div className="flex-auto w-64 rounded-md  ">
          <div className="flex flex-row-reverse">
            {!showNewPortfolioForm && (
              <button
                className="bg-highlight  rounded-md my-2 p-1"
                onClick={showCreatePortfolioForm}
              >
                <PlusIcon className="size-8  text-green " />
              </button>
            )}
          </div>
          {showNewPortfolioForm && (
            <CreatePortfolioForm
              onCreated={onNewPortfolioHandler}
              onClosed={onPortfolioFormCloseHandler}
            />
          )}

          {portfolios.map((x) => {
            return <PortfolioItem portfolio={x} />;
          })}
        </div>

        <div className="flex-auto w-32">&nbsp;</div>
      </div>
    </div>
  );
}
