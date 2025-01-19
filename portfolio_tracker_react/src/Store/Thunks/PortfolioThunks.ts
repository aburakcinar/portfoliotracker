import { createAsyncThunk } from "@reduxjs/toolkit";
import { buyStock, reserveStockOnPortfolioApi } from "../../Api";
import { fetchHoldingDetail } from "./HoldingThunks";
import { setShowNewPortfolioForm } from "../RootState";
import {
  createPortfolioApi,
  ICreatePortfolioRequest,
  listPortfoliosApi,
} from "../../Api/PortfolioV2Api";

export const fetchPortfolios = createAsyncThunk(
  "portfolio/fetch",
  async (_: void, { rejectWithValue }) => {
    try {
      return listPortfoliosApi();
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export const createPortfolio = createAsyncThunk(
  "portfolio/create",
  async (request: ICreatePortfolioRequest, { rejectWithValue, dispatch }) => {
    try {
      const result = await createPortfolioApi(request);
      if (result) {
        dispatch(fetchPortfolios());
        dispatch(setShowNewPortfolioForm(false));
      }
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

        dispatch(fetchHoldingDetail({ portfolioId, stockSymbol }));
        return result;
      }

      rejectWithValue(result);
    } catch (err) {
      rejectWithValue(err);
    }
  }
);
