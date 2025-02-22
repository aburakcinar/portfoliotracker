import React, { useEffect } from "react";
import { Outlet } from "react-router";
import { DarkModeToggle } from "./Controls/DarkModeToggle";
import { MenuContainer } from "./Controls/MenuContainer";
import { AppBreadCrumb } from "./Controls/AppBreadCrumb";
import { useAppDispatch } from "./Store/RootState";
import { fetchBankAccounts } from "./Store/BankAccount.slice";
import { fetchTransactionActionTypes } from "./Store/Transaction.slice";
import { fetchSummaryAssets } from "./Store/Assets.slice";

export function Layout() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fetchBankAccounts());
    dispatch(fetchTransactionActionTypes());
    dispatch(fetchSummaryAssets());
  }, []);

  return (
    <div className="flex h-screen bg-dark text-white">
      <div className="flex-none md:w-64 w-0 bg-nav text-white">
        <div className="w-full pt-10 text-3xl text-center  dark:text-slate-100 ">
          Portfolio Tracker
        </div>
        <div className="pt-10 pl-5">
          <DarkModeToggle />
        </div>
        <nav className="pt-10">
          <MenuContainer />
        </nav>
      </div>
      <div className="grow overflow-auto">
        <AppBreadCrumb />
        <Outlet />
      </div>
    </div>
  );
}
