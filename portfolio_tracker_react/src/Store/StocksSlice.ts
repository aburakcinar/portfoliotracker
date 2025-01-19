import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import {
  createStockItemApi,
  fetchStockItemsApi,
  ICreateStockItemRequest,
  IStockItemModel,
} from "../Api/StockItemApi";

interface IStocksState {
  list: IStockItemModel[];
}

const initialState: IStocksState = {
  list: [],
};

export const stockSlice = createSlice({
  name: "stocks",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchStockItems.fulfilled, (state, action) => {
        const { payload } = action;
        if (payload) {
          state.list = payload;
        }
      })
      .addCase(fetchStockItems.rejected, (state, action) => {
        state.list = [];
      })
      .addCase(createStockItem.fulfilled, (state, action) => {});
  },
});

export const fetchStockItems = createAsyncThunk(
  "stocks/fetch",
  async (_: void, { rejectWithValue }) => {
    try {
      const result = await fetchStockItemsApi();

      return result;
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

export const createStockItem = createAsyncThunk(
  "stocks/create",
  async (request: ICreateStockItemRequest, { rejectWithValue, dispatch }) => {
    try {
      const result = await createStockItemApi(request);

      if (result) {
        dispatch(fetchStockItems());
      }

      return result;
    } catch (err) {
      console.log(err);
      rejectWithValue(err);
    }
  }
);
