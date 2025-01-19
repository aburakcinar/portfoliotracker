import api from "../Tools/Api";

export interface IUpdateHoldingRequest {
  portfolioId: string;
  stockSymbol: string;
  holdingId: string;
  quantity: number;
  price: number;
  expenses: number;
  executeDate?: Date;
}

export interface IReportSellRequest {
  price: number;
  quantity: number;
  expenses: number;
  executeDate: Date;
}

export const updateHoldingApi = async (
  payload: IUpdateHoldingRequest
): Promise<boolean> => {
  const response = await api.put("/holding/update", payload);

  return response.data;
};

export const deleteHoldingApi = async (holdingId: string): Promise<boolean> => {
  const response = await api.delete(`/holding/delete/${holdingId}`);

  return response.data;
};

export const reportSellHoldingApi = async (
  holdingId: string,
  payload: IReportSellRequest
): Promise<boolean> => {
  const response = await api.put<boolean>(
    `/holding/reportsell/${holdingId}`,
    payload
  );

  return response.data;
};

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
