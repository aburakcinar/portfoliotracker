import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import {
  createBankAccountApi,
  fetchBankAccountsApi,
  IBankAccountModel,
  ICreateBankAccountCommand,
} from "../Api/BankAccount.api";

interface IBankAccountsState {
  list: IBankAccountModel[];
}

const initialState: IBankAccountsState = {
  list: [],
};

export const bankAccountSlice = createSlice({
  name: "bankaccount",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchBankAccounts.fulfilled, (state, action) => {
        const { payload } = action;

        if (payload) {
          state.list = payload;
        }
      })
      .addCase(createBankAccount.fulfilled, (state, action) => {});
  },
});

export const createBankAccount = createAsyncThunk(
  "bankaccount/create",
  async (command: ICreateBankAccountCommand, { rejectWithValue, dispatch }) => {
    try {
      var result = await createBankAccountApi(command);

      if (result) {
        dispatch(fetchBankAccounts());
      }
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export const fetchBankAccounts = createAsyncThunk(
  "bankaccount/list",
  async (_: void, { rejectWithValue }) => {
    try {
      return await fetchBankAccountsApi();
    } catch (err) {
      rejectWithValue(err);
    }
  }
);
