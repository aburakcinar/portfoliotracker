import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  buyStock,
  listPortfolios,
  reserveStockOnPortfolioApi,
} from "../../Api";
import { fetchHoldingDetail, fetchPortfolioHoldings } from "./HoldingThunks";

export const fetchPortfolios = createAsyncThunk(
  "portfolio/fetch",
  async (_: void, { rejectWithValue }) => {
    try {
      return listPortfolios();
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export interface IStockBuyRequest {
  portfolioId: string;
  stockSymbol: string;
  quantity: number;
  price: number;
  expenses: number;
  executeDate: Date;
}

export const reportBuyStock = createAsyncThunk(
  "portfolio/buy",
  async (command: IStockBuyRequest, { rejectWithValue, dispatch }) => {
    try {
      const result = buyStock(command);

      if (result) {
        dispatch(fetchPortfolioHoldings(command.portfolioId));
        return result;
      }

      rejectWithValue(result);
    } catch (err) {
      rejectWithValue(false);
    }
  }
);

export const reserveStockOnPortfolio = createAsyncThunk(
  "portfolio/reserveStockOnPortfolio",
  async (
    command: { portfolioId: string; stockSymbol: string },
    { rejectWithValue, dispatch }
  ) => {
    try {
      const { portfolioId, stockSymbol } = command;
      const result = await reserveStockOnPortfolioApi(portfolioId, stockSymbol);

      if (result) {
        const { portfolioId, stockSymbol } = command;

        dispatch(fetchPortfolioHoldings(portfolioId));
        dispatch(fetchHoldingDetail({ portfolioId, stockSymbol }));
        return result;
      }

      rejectWithValue(result);
    } catch (err) {
      rejectWithValue(err);
    }
  }
);
