import { Dialog } from "primereact/dialog";
import React, { useState } from "react";
import { AddInvestmentControl } from "./AddInvestmentControl";
import { Button } from "primereact/button";
import { PlusIcon, XMarkIcon } from "@heroicons/react/24/outline";

interface IAddInvestmentPopupProps {
  portfolioId: string;
  currency: string;
}

const AddInvestmentPopup: React.FC<IAddInvestmentPopupProps> = (props) => {
  const { portfolioId, currency } = props;

  const [visible, setVisible] = useState<boolean>(false);

  console.log("AddInvestmentPopup => state :", visible);

  return (
    <>
      <Button
        className="flex flex-row  bg-green p-1 items-center"
        onClick={() => setVisible(true)}
      >
        <PlusIcon className="size-6 text-black" />
        <span className=" text-black">Investment</span>
      </Button>
      <Dialog
        visible={visible}
        modal
        className="bg-highlight w-[600px]"
        onHide={() => {
          if (!visible) return;
          setVisible(false);
        }}
        content={() => (
          <div className="flex flex-col bg-highlight">
            <div className="flex flex-row w-full bg-green">
              <div className="grow text-lg p-1">Add New Investment</div>
              <div className="grow-0">
                <Button
                  className="p-1 items-center"
                  onClick={() => setVisible(false)}
                >
                  <XMarkIcon className="size-6 text-black" />
                </Button>
              </div>
            </div>
            <div className="m-1">
              <AddInvestmentControl
                currency={currency}
                portfolioId={portfolioId}
              />
            </div>
          </div>
        )}
      ></Dialog>
    </>
  );
};

export { type IAddInvestmentPopupProps, AddInvestmentPopup };
