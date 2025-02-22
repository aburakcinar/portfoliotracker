import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  fetchTransactionActionTypesApi,
  fetchTransactionsApi,
  ITransactionActionType,
  TransactionActionTypeCategory,
} from "../Api/Transaction.api";
import {
  addTransactionApi,
  IAddTransactionCommand,
  IBankTransactionGroupModel,
  InOut,
  listBankAccountTransactionsApi,
} from "../Api/BankTransactions.api";

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

export interface IBankAccountTransactionCreateModel {
  index: number;
  price: number;
  quantity: number;
  inOut: InOut;
  actionTypeCode: string;
  actionTypeCategory: TransactionActionTypeCategory;
  description: string;
}

interface ITransactionsState {
  list: ITransactionItem[];
  displayList: ITransactionItem[];
  loading: boolean;
  pageCount: number;
  pageSize: number;
  pageIndex: number;
  recordCount: number;
  actionTypeCategories: {
    label: string;
    category: TransactionActionTypeCategory;
  }[];
  actionTypes: ITransactionActionType[];
  bankTransactions: IBankTransactionGroupModel[];
}

const initialState: ITransactionsState = {
  list: [],
  displayList: [],
  loading: false,
  pageCount: 0,
  pageSize: 10,
  pageIndex: 0,
  recordCount: 0,
  actionTypeCategories: [
    { label: "Incoming", category: TransactionActionTypeCategory.Incoming },
    { label: "Outgoing", category: TransactionActionTypeCategory.Outgoing },
    { label: "Fee", category: TransactionActionTypeCategory.Fee },
    { label: "Tax", category: TransactionActionTypeCategory.Tax },
  ],
  actionTypes: [],
  bankTransactions: [],
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

export const fetchTransactionActionTypes = createAsyncThunk(
  "transactions/fetchTransactionActionTypes",
  async (_: void, { rejectWithValue }) => {
    try {
      return await fetchTransactionActionTypesApi();
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export const addTransaction = createAsyncThunk(
  "transactions/addTransaction",
  async (command: IAddTransactionCommand, { rejectWithValue, dispatch }) => {
    try {
      const response = await addTransactionApi(command);

      if (response) {
        dispatch(listBankAccountTransactions(command.bankAccountId));

        return true;
      }
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export const listBankAccountTransactions = createAsyncThunk(
  "transactions/listBankAccountTransactions",
  async (bankAccountId: string, { rejectWithValue }) => {
    try {
      const response = await listBankAccountTransactionsApi(bankAccountId);
      if (response) {
        return response;
      }
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
      })
      .addCase(fetchTransactionActionTypes.fulfilled, (state, action) => {
        const { payload } = action;

        if (payload) {
          state.actionTypes = payload;
        }
      })
      .addCase(addTransaction.fulfilled, (state, action) => {})
      .addCase(listBankAccountTransactions.fulfilled, (state, action) => {
        const { payload } = action;

        if (payload && payload.length > 0) {
          const { bankAccountId } = payload[0];

          state.bankTransactions = [
            ...state.bankTransactions.filter(
              (x) => x.bankAccountId !== bankAccountId
            ),
            ...payload,
          ];
        }
      });
  },
});

export const { setPageIndex } = transactionsSlice.actions;
