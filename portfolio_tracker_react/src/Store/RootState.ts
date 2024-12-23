import { configureStore, createSlice } from "@reduxjs/toolkit";
import { IPortfolioModel } from "./Models";
import {
  fetchPortfolios,
  reportBuyStock,
} from "./PortfolioSlice/PortfolioThunks";
import { useDispatch, useSelector } from "react-redux";

interface IPortfolioState {
  portfolios: IPortfolioModel[];
}

const initialPortfolioState: IPortfolioState = {
  portfolios: [],
};

const portfolioSlice = createSlice({
  name: "portfolio",
  initialState: initialPortfolioState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchPortfolios.fulfilled, (state, action) => {
        const { payload } = action;
        if (payload) {
          state.portfolios = payload;
        }
      })
      .addCase(reportBuyStock.fulfilled, (state, action) => {})
      .addCase(reportBuyStock.rejected, (state, action) => {});
  },
});

export const store = configureStore({
  reducer: {
    portfolios: portfolioSlice.reducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();
