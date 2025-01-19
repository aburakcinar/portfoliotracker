import { createAsyncThunk } from "@reduxjs/toolkit";
import { listHoldingDetails, listHoldingsByPortfolio } from "../../Api";
import {
  deleteHoldingApi,
  IReportSellRequest,
  IUpdateHoldingRequest,
  reportSellHoldingApi,
  updateHoldingApi,
} from "../../Api/HoldingsApi";

// export const fetchPortfolioHoldings = createAsyncThunk(
//   "portfolio/holdings",
//   async (portfolioId: string, { rejectWithValue }) => {
//     try {
//       const data = await listHoldingsByPortfolio(portfolioId);

//       if (data) {
//         return { portfolioId, data };
//       }
//     } catch (err) {
//       rejectWithValue(err);
//     }
//   }
// );

export const updateHolding = createAsyncThunk(
  "portfolio/updateHolding",
  async (command: IUpdateHoldingRequest, { rejectWithValue, dispatch }) => {
    try {
      const result = await updateHoldingApi(command);

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

export const deleteHolding = createAsyncThunk(
  "portfolio/deleteHolding",
  async (
    command: { portfolioId: string; stockSymbol: string; holdingId: string },
    { rejectWithValue, dispatch }
  ) => {
    try {
      const result = await deleteHoldingApi(command.holdingId);

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

export const fetchHoldingDetail = createAsyncThunk(
  "portfolio/fetchHoldingDetail",
  async (
    payload: { portfolioId: string; stockSymbol: string },
    { rejectWithValue }
  ) => {
    try {
      const { portfolioId, stockSymbol } = payload;

      const result = await listHoldingDetails(portfolioId, stockSymbol);

      return {
        ...payload,
        result,
      };
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export const reportSellHolding = createAsyncThunk(
  "portfolio/reportSellHolding",
  async (
    payload: {
      portfolioId: string;
      stockSymbol: string;
      holdingId: string;
      request: IReportSellRequest;
    },
    { rejectWithValue, dispatch }
  ) => {
    try {
      const { portfolioId, stockSymbol, holdingId, request } = payload;

      const result = await reportSellHoldingApi(holdingId, request);

      if (result) {
        dispatch(fetchHoldingDetail({ portfolioId, stockSymbol }));
      }
    } catch (err) {
      rejectWithValue(err);
    }
  }
);
