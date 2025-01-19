import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { HomeIcon, PresentationChartBarIcon } from "@heroicons/react/24/solid";
import { ReactNode } from "react";
import { flattenMenuItem } from "../Tools/Navigation";

export interface IMenuItem {
  id: string;
  text: string;
  link: string;
  children?: IMenuItem[];
}

export interface IMenuState {
  list: IMenuItem[];
  flattenList: IMenuItem[];
}

const initialState: IMenuState = {
  list: [
    {
      id: "home",
      text: "Home",
      link: "/",
    },
    {
      id: "portfolios",
      text: "Portfolios",
      link: "/portfolios",
    },

    {
      id: "assets",
      text: "Assets",
      link: "/assets",
      children: [
        {
          id: "assets/stocks",
          text: "Stocks",
          link: "/assets/stocks",
        },
        {
          id: "assets/etfs",
          text: "ETFs",
          link: "/assets/etfs",
        },
        {
          id: "assets/commodities",
          text: "Commodities",
          link: "/assets/commodities",
        },
      ],
    },

    {
      id: "bankaccounts",
      text: "Bank Accounts",
      link: "/bankaccounts",
    },
  ],
  flattenList: [],
};

export const menuSlice = createSlice({
  name: "menu",
  initialState,
  reducers: {
    generateFlattenList: (state, action: PayloadAction<void>) => {
      state.flattenList = flattenMenuItem(state.list);
    },
    addMenuItem: (state, action: PayloadAction<IMenuItem>) => {
      state.flattenList.push(action.payload);
    },
  },
});

export const { generateFlattenList, addMenuItem } = menuSlice.actions;
