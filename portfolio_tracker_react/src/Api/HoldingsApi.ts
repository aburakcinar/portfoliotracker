import api from "./Api";

export enum AssetTypes {
  Stock = 1,
  ETF = 2,
  Commodity = 3,
  Bond = 4,
  Crypto = 5,
}

export interface IHoldingListModel {
  id: string;
  portfolioId: string;
  assetId: string;
  assetName: string;
  assetTickerSymbol: string;
  assetPrice: number;
  assetType: number;
  assetTypeName: string;
  exchangeCode: string;
  countryCode: string;
  currencyCode: string;
  currencyName: string;
  currencySymbol: string;
}

export interface IHoldingAggregateModel {
  portfolioId: string;
  assetId: string;
  assetName: string;
  assetTickerSymbol: string;
  assetPrice: number;
  assetType: number;
  assetTypeName: string;
  exchangeCode: string;
  countryCode: string;
  currencyCode: string;
  currencyName: string;
  currencySymbol: string;

  totalQuantity: number;
  totalCost: number;
  averagePrice: number;
  totalExpenses: number;

  color: string;
}

export const fetchHoldingsByPortfolioIdApi = async (
  portfolioId: string
): Promise<IHoldingAggregateModel[]> => {
  const response = await api.get<IHoldingAggregateModel[]>(
    `holding/listbyportfolio/${portfolioId}`
  );

  return response.data;
};

export interface IHoldingDetailModel {
  id: string;
  quantity: number;
  price: number;
  total: number;
  currencyCode: string;
  currencySymbol: string;
  executeDate: Date;
}

export const fetchHoldingDetailApi = async (
  portfolioId: string,
  assetId: string
): Promise<IHoldingDetailModel[]> => {
  const response = await api.get(
    `holding/listbyportfolio/${portfolioId}/asset/${assetId}`
  );

  return response.data;
};

export interface IHoldingAssetTransactionModel {
  id: string;
  executeDate: Date;
  description: string;
  quantity: number;
  price: number;
  total: number;
  expenses: number;
  taxes: number;
  currencyCode: string;
  currencySymbol: string;
}

export const fetchHoldingAssetTransactionsApi = async (
  portfolioId: string,
  assetId: string
): Promise<IHoldingAssetTransactionModel[]> => {
  const response = await api.get(
    `holding/transactions/portfolio/${portfolioId}/asset/${assetId}`
  );

  return response.data;
};

export interface IAddHoldingCommand {
  portfolioId: string;
  assetId: string;
  quantity: number;
  price: number;
  expenses: number;
  executeDate: string;
  executeDateTime: Date;
}

export const addHoldingApi = async (
  command: IAddHoldingCommand
): Promise<boolean> => {
  const response = await api.post<boolean>("holding/reportbuy", command);

  return response.data;
};

export interface ISellHoldingCommand {
  portfolioId: string;
  assetId: string;
  quantity: number;
  price: number;
  expenses: number;
  taxes: number;
  executeDate: string;
  executeDateTime: Date;
}

export const sellHoldingApi = async (
  command: ISellHoldingCommand
): Promise<boolean> => {
  const response = await api.post<boolean>("holding/reportsell", command);
  return response.data;
};
