import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { flattenMenuItem } from "../Tools/Navigation";

export interface IMenuItem {
  id: string;
  text: string;
  link: string;
  children?: IMenuItem[];
  visible?: boolean;
  isDefault?: boolean;
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
      isDefault: true,
    },
    {
      id: "portfolios",
      text: "Portfolios",
      link: "/portfolios",
      isDefault: true,
      children: [
        {
          id: "portfolios/:portfolioId/addholding",
          text: "Add Holding",
          link: "/portfolios/:portfolioId/addholding",
          visible: false,
          isDefault: true,
        },
      ],
    },
    {
      id: "assets",
      text: "Assets",
      link: "/assets",
      isDefault: true,
      children: [
        {
          id: "assets/stocks",
          text: "Stocks",
          link: "/assets/stocks",
          isDefault: true,
          children: [
            {
              id: "assets/stocks/new",
              text: "New Stock",
              link: "/assets/stocks/new",
              visible: false,
              isDefault: true,
            },
          ],
        },
        {
          id: "assets/etfs",
          text: "ETFs",
          link: "/assets/etfs",
          isDefault: true,
        },
        {
          id: "assets/commodities",
          text: "Commodities",
          link: "/assets/commodities",
          isDefault: true,
        },
      ],
    },

    {
      id: "bankaccounts",
      text: "Bank Accounts",
      link: "/bankaccounts",
      isDefault: true,
      children: [
        {
          id: "bankaccounts/new",
          text: "New Bank Account",
          link: "/bankaccounts/new",
          visible: false,
          isDefault: true,
        },
        {
          id: "bankaccounts/detail",
          text: "Detail",
          link: "/bankaccounts/detail",
          visible: false,
          isDefault: true,
        },
        {
          id: "bankaccounts/:id/transactions",
          text: "Transactions",
          link: "/bankaccounts/:id/transactions",
          visible: false,
          isDefault: true,
        },
        {
          id: "bankaccounts/:id/transactions/import",
          text: "Import Transactions",
          link: "/bankaccounts/:id/transactions/import",
          visible: false,
          isDefault: true,
        },
      ],
    },
    {
      id: "exchangerates",
      text: "Exchange Rates",
      link: "/exchangerates",
      isDefault: true,
    },
    {
      id: "admin/dashboard",
      text: "Dashboard",
      link: "/admin/dashboard",
      isDefault: true,
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
    addMenuItem: (state, action: PayloadAction<IMenuItem | IMenuItem[]>) => {
      const { payload } = action;
      const items = Array.isArray(payload) ? payload : [payload];
      const newIds = items.map((x) => x.id);
      const newList = state.flattenList.filter((x) => !newIds.includes(x.id));
      state.flattenList = [...newList, ...items];
    },
    removeMenuItem: (state, action: PayloadAction<IMenuItem | IMenuItem[]>) => {
      const { payload } = action;
      const items = Array.isArray(payload) ? payload : [payload];
      const newIds = items.map((x) => x.id);
      const newList = state.flattenList.filter((x) => !newIds.includes(x.id));
      state.flattenList = [...newList];
    },
  },
});

export const { generateFlattenList, addMenuItem, removeMenuItem } =
  menuSlice.actions;
