import React from "react";
import {
  PresentationChartBarIcon,
  NumberedListIcon,
} from "@heroicons/react/24/outline";
import { HomeIcon } from "@heroicons/react/24/solid";
import { Link, Outlet } from "react-router";

export function Layout() {
  return (
    <div className="flex h-screen bg-dark text-white">
      <div className="flex-none w-64  bg-nav text-white">
        <div className="w-full text-center text-lg text-slate-100">
          Portfolio Tracker
        </div>
        <nav className="pt-16">
          <ul className="text-white ">
            <Link to="/">
              <li className="p-2 hover:bg-highlight">
                <div className="flex text-green">
                  <HomeIcon className="size-9 flex-none px-2" />
                  <span className="grow py-1">Home</span>
                </div>
              </li>
            </Link>
            <Link to="/Portfolio">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <PresentationChartBarIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Portfolio</span>
                </div>
              </li>
            </Link>
            <Link to="/Transactions">
              <li className="p-2 hover:bg-highlight">
                <div className="flex">
                  <NumberedListIcon className="size-9 flex-none px-2 " />
                  <span className="grow py-1">Transactions</span>
                </div>
              </li>
            </Link>
          </ul>
        </nav>
      </div>
      <div className="grow  overflow-auto">
        <div className="overflow-auto">
          <Outlet />
        </div>
      </div>
    </div>
  );
}
