import api, { toQueryString } from "../Tools/Api";

const listCountries = async (): Promise<string[]> => {
  const response = await api.get<string[]>("/asset/countries");

  return response.data;
};

const listCurrencies = async (): Promise<string[]> => {
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

const searchStocks = async (
  request: ISearchStockRequest
): Promise<IStockItem[]> => {
  const queryString = toQueryString(request);
  const response = await api.get<IStockItem[]>(
    `/asset/stocks/search?${queryString}`
  );

  return response.data;
};

export { listCountries, listCurrencies, searchStocks };
