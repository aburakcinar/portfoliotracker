import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { IStockItem } from "../Api";
import {
  AssetTypes,
  createAssetStockApi,
  fetchSummaryAssetsApi,
  IAssetModel,
  IAssetSummaryModel,
  ICreateAssetStockRequest,
  ISearchAssetBaseRequest,
  ISearchAssetsRequest,
  searchAssetsApi,
} from "../Api/AssetApi";
import { RootState } from "./RootState";

interface IAssetsState {
  summary: IAssetSummaryModel[];
  list: IStockItem[];
  stocks: IAssetModel[];
  stocksSearchRequest: ISearchAssetBaseRequest | null;
  etfs: IAssetModel[];
  etfsSearchRequest: ISearchAssetBaseRequest | null;
  commodities: IAssetModel[];
  commoditiesSearchRequest: ISearchAssetBaseRequest | null;
  bonds: IAssetModel[];
  bondsSearchRequest: ISearchAssetBaseRequest | null;
  cryptos: IAssetModel[];
  cryptosSearchRequest: ISearchAssetBaseRequest | null;
}

const initialState: IAssetsState = {
  summary: [],
  list: [],
  stocks: [],
  stocksSearchRequest: null,
  etfs: [],
  etfsSearchRequest: null,
  commodities: [],
  commoditiesSearchRequest: null,
  bonds: [],
  bondsSearchRequest: null,
  cryptos: [],
  cryptosSearchRequest: null,
};

export const assetsSlice = createSlice({
  name: "assets",
  initialState,
  reducers: {
    storeSearchParameters: (
      state,
      action: PayloadAction<ISearchAssetsRequest>
    ) => {
      const { payload } = action;

      if (payload.assetType === AssetTypes.Stock) {
        state.stocksSearchRequest = { ...payload };
      } else if (payload.assetType === AssetTypes.ETF) {
        state.etfsSearchRequest = { ...payload };
      } else if (payload.assetType === AssetTypes.Commodity) {
        state.commoditiesSearchRequest = { ...payload };
      } else if (payload.assetType === AssetTypes.Bond) {
        state.bondsSearchRequest = { ...payload };
      } else if (payload.assetType === AssetTypes.Crypto) {
        state.cryptosSearchRequest = { ...payload };
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchSummaryAssets.fulfilled, (state, action) => {
        const { payload } = action;

        if (payload) {
          state.summary = payload;
        }
      })
      .addCase(searchAssets.fulfilled, (state, action) => {
        const { payload } = action;

        if (payload) {
          const { request, response } = payload;
          const { assetType } = request;

          if (assetType === AssetTypes.Stock) {
            state.stocks = response;
            state.stocksSearchRequest = { ...request };
          } else if (assetType === AssetTypes.ETF) {
            state.etfs = response;
            state.etfsSearchRequest = { ...request };
          } else if (assetType === AssetTypes.Commodity) {
            state.commodities = response;
            state.commoditiesSearchRequest = { ...request };
          } else if (assetType === AssetTypes.Bond) {
            state.bonds = response;
            state.bondsSearchRequest = { ...request };
          } else if (assetType === AssetTypes.Crypto) {
            state.cryptos = response;
            state.cryptosSearchRequest = { ...request };
          }
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
      rejectWithValue(err);
    }
  }
);

export const createStockAsset = createAsyncThunk(
  "assets/createStock",
  async (
    request: ICreateAssetStockRequest,
    { rejectWithValue, dispatch, getState }
  ) => {
    try {
      const result = await createAssetStockApi(request);

      if (result) {
        const state = getState() as RootState;

        dispatch(fetchSummaryAssets());

        const searchRequest = state.assets.stocksSearchRequest ?? {
          searchText: "",
          pageIndex: 0,
          pageSize: 20,
        };

        dispatch(
          searchAssets({
            ...searchRequest,
            assetType: AssetTypes.Stock,
          })
        );
      }
    } catch (err) {
      rejectWithValue(err);
    }
  }
);
