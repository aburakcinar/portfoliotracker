import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { fetchTransactionsApi } from "../Api/TransactionApi";

export interface ITransactionItem {
  id: string;
  price: number;
  quantity: number;
  total: number;
  executeDate: Date;
  inOut: number;
  transactionType: number;
  description: string;
  transactionGroupId: string;
  holdingId: string;
  portfolioId: string;
  portfolioName: string;
  currencyCode: string;
  stockId: string;
  stockName: string;
  stockSymbol: string;
}

interface ITransactionsState {
  list: ITransactionItem[];
  displayList: ITransactionItem[];
  loading: boolean;
  pageCount: number;
  pageSize: number;
  pageIndex: number;
  recordCount: number;
}

const initialState: ITransactionsState = {
  list: [],
  displayList: [],
  loading: false,
  pageCount: 0,
  pageSize: 10,
  pageIndex: 0,
  recordCount: 0,
};

// <Thunks>
export const fetchTransactions = createAsyncThunk(
  "transactions/fetchTransactions",
  async (_: void, { rejectWithValue }) => {
    try {
      return await fetchTransactionsApi();
    } catch (err) {
      rejectWithValue(err);
    }
  }
);
// </Thunks>

const prepareDisplayList = (state: ITransactionsState): ITransactionItem[] => {
  const { list } = state;
  const { pageIndex, pageSize } = state;

  return [...list.slice(pageIndex * pageSize, (pageIndex + 1) * pageSize)];
};

export const transactionsSlice = createSlice({
  name: "transactions",
  initialState,
  reducers: {
    setPageIndex: (state, action: PayloadAction<number>) => {
      const { list, pageCount } = state;
      const { payload } = action;
      const maxPageCount = Math.round(list.length / pageCount);

      if (payload < maxPageCount) {
        state.pageIndex = payload;

        state.displayList = prepareDisplayList(state);
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchTransactions.fulfilled, (state, action) => {
        if (action.payload) {
          state.list = action.payload;
        }
        state.loading = false;
        state.displayList = prepareDisplayList(state);
        state.recordCount = state.list.length;
      })
      .addCase(fetchTransactions.pending, (state, action) => {
        state.loading = true;
      })
      .addCase(fetchTransactions.rejected, (state, action) => {
        state.loading = false;
      });
  },
});

export const { setPageIndex } = transactionsSlice.actions;
