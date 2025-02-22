import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";

import { assetsSlice } from "./Assets.slice";
import { transactionsSlice } from "./Transaction.slice";
import { menuSlice } from "./Menu.slice";
import { bankAccountSlice } from "./BankAccount.slice";
import { portfolioSlice } from "./Portfolio.slice";

export const store = configureStore({
  reducer: {
    portfolios: portfolioSlice.reducer,
    assets: assetsSlice.reducer,
    transactions: transactionsSlice.reducer,
    menu: menuSlice.reducer,
    bankAccounts: bankAccountSlice.reducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();
