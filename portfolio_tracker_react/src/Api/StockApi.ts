import api from "../Tools/Api";

interface IStockItem {
  id: string;
  stockExchange: string;
  symbol: string;
  name: string;
}

const listStocks = async (): Promise<IStockItem[]> => {
  const response = await api.get<IStockItem[]>("/stock");

  return response.data;
};

export { type IStockItem, listStocks };
