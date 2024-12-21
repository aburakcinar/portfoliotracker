import api from "../Tools/Api";

interface ICurrencyItem {
  code: string;
  name: string;
  symbol: string;
}

const getCurrencies = async (): Promise<ICurrencyItem[]> => {
  const response = await api.get<ICurrencyItem[]>("/portfolio/currencies");

  return response.data;
};

export { type ICurrencyItem, getCurrencies };
