import { configureStore, createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  IHoldingDetailModel,
  IPortfolioHoldingModel,
  IPortfolioModel,
} from "./Models";
import {
  createPortfolio,
  fetchHoldingDetail,
  fetchPortfolios,
  reportBuyStock,
  reportSellHolding,
  reserveStockOnPortfolio,
  updateHolding,
} from "./Thunks";
import { useDispatch, useSelector } from "react-redux";
import { assetsSlice } from "./AssetsSlice";
import { transactionsSlice } from "./TransactionSlice";
import { stockSlice } from "./StocksSlice";
import { menuSlice } from "./MenuSlice";

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

const portfolioSlice = createSlice({
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
      })
      .addCase(reportBuyStock.fulfilled, (state, action) => {})
      .addCase(reportBuyStock.rejected, (state, action) => {})
      // .addCase(fetchPortfolioHoldings.fulfilled, (state, action) => {
      //   const { payload } = action;

      //   if (payload) {
      //     const { portfolioId, data } = payload;
      //     state.holdings[portfolioId] = data;
      //   }
      // })
      .addCase(reserveStockOnPortfolio.fulfilled, (state, action) => {})
      .addCase(updateHolding.fulfilled, (state, action) => {})
      .addCase(fetchHoldingDetail.fulfilled, (state, action) => {
        const { payload } = action;

        if (payload) {
          const { portfolioId, stockSymbol, result } = payload;
          state.holdingDetails[`${portfolioId}_${stockSymbol}`] = result;
        }
      })
      .addCase(reportSellHolding.fulfilled, (state, action) => {});
  },
});

export const {
  setShowReportSellDialog,
  setSelectedReportSellItem,
  setShowNewPortfolioForm,
} = portfolioSlice.actions;

export const store = configureStore({
  reducer: {
    portfolios: portfolioSlice.reducer,
    assets: assetsSlice.reducer,
    transactions: transactionsSlice.reducer,
    stocks: stockSlice.reducer,
    menu: menuSlice.reducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();
