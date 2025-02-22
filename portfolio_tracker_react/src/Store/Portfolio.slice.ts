import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  IHoldingDetailModel,
  IPortfolioHoldingModel,
  IPortfolioModel,
} from "./Models";
import {
  createPortfolioApi,
  ICreatePortfolioRequest,
  IUpdatePortfolioRequest,
  listPortfoliosApi,
  updatePortfolioApi,
} from "../Api/Portfolio.api";

interface IPortfolioState {
  portfolios: IPortfolioModel[];
  holdings: { [portfolioId: string]: IPortfolioHoldingModel[] };
  holdingDetails: {
    [portfolioId_StockSymbol: string]: IHoldingDetailModel[];
  };
  showReportSellDialog: boolean;
  selectedReportSellItem: IHoldingDetailModel | null;
  showNewPortfolioForm: boolean;
}

const initialPortfolioState: IPortfolioState = {
  portfolios: [],
  holdings: {},
  holdingDetails: {},
  showReportSellDialog: false,
  selectedReportSellItem: null,
  showNewPortfolioForm: false,
};

export const portfolioSlice = createSlice({
  name: "portfolio",
  initialState: initialPortfolioState,
  reducers: {
    setShowNewPortfolioForm: (state, action: PayloadAction<boolean>) => {
      state.showNewPortfolioForm = action.payload;
    },
    setShowReportSellDialog: (state, action: PayloadAction<boolean>) => {
      state.showReportSellDialog = action.payload;
    },
    setSelectedReportSellItem: (
      state,
      action: PayloadAction<IHoldingDetailModel | null>
    ) => {
      state.selectedReportSellItem = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(createPortfolio.fulfilled, (state, action) => {})
      .addCase(fetchPortfolios.fulfilled, (state, action) => {
        const { payload } = action;
        if (payload) {
          state.portfolios = payload;
        }
      });
  },
});

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

export const updatePortfolio = createAsyncThunk(
  "portfolio/update",
  async (request: IUpdatePortfolioRequest, { rejectWithValue, dispatch }) => {
    try {
      const result = await updatePortfolioApi(request);
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

export const {
  setShowReportSellDialog,
  setSelectedReportSellItem,
  setShowNewPortfolioForm,
} = portfolioSlice.actions;
