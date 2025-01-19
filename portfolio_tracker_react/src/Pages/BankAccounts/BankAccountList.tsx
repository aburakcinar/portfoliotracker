import React from "react";
import { useNavigate } from "react-router";
import { PlusIcon } from "@heroicons/react/24/outline";

export const BankAccountList: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="flex flex-col">
      <h2 className="text-green pb-8 text-5xl">Bank Accounts</h2>
      <div className="flex flex-row-reverse">
        <button className="bg-green h-10 flex flex-row p-2">
          <PlusIcon className="size-5" />
          <span>New Account</span>
        </button>
      </div>
    </div>
  );
};
