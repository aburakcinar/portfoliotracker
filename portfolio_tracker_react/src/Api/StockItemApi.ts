import api from "../Tools/Api";

export interface IStockItemModel {
  fullCode: string;
  stockExchange: string;
  symbol: string;
  name: string;
  description: string;
  localeCode: string;
  webSite: string;
}

export interface ICreateStockItemRequest {
  stockExchangeCode: string;
  symbol: string;
  name: string;
  description: string;
  localeCode: string;
  webSite: string;
}

const createStockItemApi = async (
  item: ICreateStockItemRequest
): Promise<boolean> => {
  const response = await api.post<boolean>("/stock/v2", item);

  return response.data;
};

const updateteStockItemApi = async (
  item: IStockItemModel
): Promise<boolean> => {
  const response = await api.put<boolean>("/stock/v2", item);

  return response.data;
};

const deleteStockItemApi = async (fullStockCode: string): Promise<boolean> => {
  const response = await api.delete(`/stock/v2/${fullStockCode}`);

  return response.data;
};

const getStockItemApi = async (
  fullStockCode: string
): Promise<IStockItemModel> => {
  const response = await api.get<IStockItemModel>(`/stock/v2/${fullStockCode}`);

  return response.data;
};

const fetchStockItemsApi = async (): Promise<IStockItemModel[]> => {
  const response = await api.get<IStockItemModel[]>("/stock/v2/list");

  return response.data;
};

export {
  createStockItemApi,
  updateteStockItemApi,
  deleteStockItemApi,
  getStockItemApi,
  fetchStockItemsApi,
};
