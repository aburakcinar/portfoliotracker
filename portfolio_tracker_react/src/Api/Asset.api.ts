import api, { toQueryString } from "../Tools/Api";

export const listCountries = async (): Promise<string[]> => {
  const response = await api.get<string[]>("/asset/countries");

  return response.data;
};

export const listCurrencies = async (): Promise<string[]> => {
  const response = await api.get<string[]>("/asset/currencies");

  return response.data;
};

export interface IStockItem {
  id: number;
  country: string;
  name: string;
  fullName: string;
  tag: string;
  isin: string;
  currency: string;
  symbol: string;
}

export interface ISearchStockRequest {
  searchText: string;
  countries?: string[];
  pageIndex: number;
  pageSize: number;
}

export const searchStocks = async (
  request: ISearchStockRequest
): Promise<IStockItem[]> => {
  const queryString = toQueryString(request);
  const response = await api.get<IStockItem[]>(
    `/asset/stocks/search?${queryString}`
  );

  return response.data;
};

export interface IAssetSummaryModel {
  assetTypeId: number;
  assetType: string;
  title: string;
  count: number;
}

export const fetchSummaryAssetsApi = async (): Promise<
  IAssetSummaryModel[]
> => {
  const response = await api.get<IAssetSummaryModel[]>("/asset/summary");

  return response.data;
};

export enum AssetTypes {
  Stock = 1,
  ETF = 2,
  Commodity = 3,
  Bond = 4,
  Crypto = 5,
}

export interface ICreateAssetCommand {
  assetType: AssetTypes;
  tickerSymbol: string;
  name: string;
  exchangeCode: string;
  currencyCode: string;
}

export const createAssetApi = async (
  command: ICreateAssetCommand
): Promise<boolean> => {
  const response = await api.post<boolean>("asset/stocks", command);

  return response.data;
};

export interface ISearchAssetBaseRequest {
  searchText: string;
  pageIndex: number;
  pageSize: number;
}

export interface ISearchAssetsRequest extends ISearchAssetBaseRequest {
  assetType: AssetTypes | null;
}

export interface IAssetModel {
  id: string;
  tickerSymbol: string;
  exchangeCode: string;
  exchangeName: string;
  exchangeCountryCode: string;
  currencyCode: string;
  currencyName: string;
  currencySymbol: string;
  name: string;
  description: string;
  isin: string;
  wkn: string;
  webSite: string;
  created: Date;
  updated?: Date;
  price: number;
}

export const searchAssetsApi = async (
  request: ISearchAssetsRequest
): Promise<IAssetModel[]> => {
  const response = await api.get<IAssetModel[]>(
    `asset/search?${toQueryString(request)}`
  );

  return response.data;
};

export const getAssetApi = async (id: string): Promise<IAssetModel | null> => {
  const response = await api.get<IAssetModel | null>(`asset/get/${id}`);

  return response.data;
};

export const updateAssetFieldApi = async (
  assetId: string,
  fieldName: string,
  value: string
): Promise<boolean> => {
  const response = await api.put<boolean>("asset/updatefield", {
    assetId,
    fieldName,
    value,
  });

  return response.data;
};

export interface IHoldingAssetSummaryModel {
  id: string;
  symbol: string;
  name: string;
  price: number;
  assetCurrency: string;
  priceInPortfolioCurrency: number;
  portfolioCurrency: string;
  quantity: number;
  totalProfitLoss: number;
  realizedProfitLoss: number;
  unrealizedProfitLoss: number;
  totalFees: number;
  totalTaxes: number;
}

export const getHoldingAssetSummaryApi = async (
  portfolioId: string,
  assetId: string
): Promise<IHoldingAssetSummaryModel | null> => {
  const response = await api.get<IHoldingAssetSummaryModel | null>(
    `holding/asset/summary/portfolio/${portfolioId}/asset/${assetId}`
  );
  return response.data;
};
