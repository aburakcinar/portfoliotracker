import { createAsyncThunk } from "@reduxjs/toolkit";
import { buyStock, listPortfolios } from "../../Api";

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
  expense: number;
  executeDate: Date;
}

export const reportBuyStock = createAsyncThunk(
  "portfolio/buy",
  async (command: IStockBuyRequest, { rejectWithValue }) => {
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
