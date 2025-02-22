import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  fetchSummaryAssetsApi,
  IAssetModel,
  IAssetSummaryModel,
  ISearchAssetBaseRequest,
  ISearchAssetsRequest,
  searchAssetsApi,
} from "../Api/Asset.api";

interface IAssetsState {
  summary: IAssetSummaryModel[];
  searchParameters: ISearchAssetBaseRequest;
}

const initialState: IAssetsState = {
  summary: [],
  searchParameters: {
    searchText: "",
    pageIndex: 0,
    pageSize: 20,
  },
};

export const assetsSlice = createSlice({
  name: "assets",
  initialState,
  reducers: {
    storeSearchParameters: (
      state,
      action: PayloadAction<ISearchAssetBaseRequest>
    ) => {
      const { payload } = action;

      state.searchParameters = payload;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchSummaryAssets.fulfilled, (state, action) => {
      const { payload } = action;

      if (payload) {
        state.summary = payload;
      }
    });
  },
});

export const { storeSearchParameters } = assetsSlice.actions;

export const fetchSummaryAssets = createAsyncThunk(
  "assets/summary",
  async (_: void, { rejectWithValue }) => {
    try {
      return await fetchSummaryAssetsApi();
    } catch (err) {
      rejectWithValue(err);
    }
  }
);

interface ISearchAssetsResponse {
  request: ISearchAssetsRequest;
  response: IAssetModel[];
}

export const searchAssets = createAsyncThunk<
  ISearchAssetsResponse | undefined,
  ISearchAssetsRequest
>(
  "assets/searchassets",
  async (request: ISearchAssetsRequest, { rejectWithValue }) => {
    try {
      const response = await searchAssetsApi(request);

      if (response) {
        return { request, response };
      }

      rejectWithValue("fetch error");
    } catch (err) {
      console.log(err);
      rejectWithValue(err);
    }
  }
);
