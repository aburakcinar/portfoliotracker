import React from "react";
import { Outlet } from "react-router";
import { DarkModeToggle } from "./Controls/DarkModeToggle";
import { MenuContainer } from "./Controls/MenuContainer";
import { AppBreadCrumb } from "./Controls/AppBreadCrumb";

export function Layout() {
  return (
    <div className="flex h-screen bg-dark text-white">
      <div className="flex-none md:w-64 w-0 bg-nav text-white">
        <div className="w-full pt-10 text-3xl text-center  dark:text-slate-100 ">
          Portfolio Tracker
        </div>
        <nav className="pt-10">
          <MenuContainer />
          {/* <ul className="text-white ">
            <Link to="/">
              <li className="p-2 hover:bg-highlight">
                <div className="flex text-green">
                  <HomeIcon className="size-9 flex-none px-2" />
                  <span className="grow py-1">Home</span>
                </div>
              </li>
            </Link>
            <Link to="/portfolio">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <PresentationChartBarIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Portfolio</span>
                </div>
              </li>
            </Link>
            <Link to="/transactions">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <NumberedListIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Transactions</span>
                </div>
              </li>
            </Link>
            <Link to="/stocks">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <DocumentCurrencyDollarIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Stocks</span>
                </div>
              </li>
            </Link>
            <Link to="/exchanges">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <DocumentCurrencyDollarIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Exchanges</span>
                </div>
              </li>
            </Link>
            <Link to="/controls">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <NumberedListIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Example Controls</span>
                </div>
              </li>
            </Link>
          </ul> */}
        </nav>
        <DarkModeToggle />
      </div>
      <div className="grow overflow-auto">
        <AppBreadCrumb />
        <Outlet />
      </div>
    </div>
  );
}
