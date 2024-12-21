import React from "react";
import { PlusIcon, MinusIcon } from "@heroicons/react/24/solid";

interface IQuantityProps {}

const Quantity: React.FC<IQuantityProps> = (props) => {
  return (
    <div className="border-b-nav border-1 flex flex-row">
      <PlusIcon className="grow-0 size-4" />
      <input type="number" className="grow" />
      <MinusIcon className="grow-0 size-4" />
    </div>
  );
};

export { type IQuantityProps, Quantity };
