import api from "../Tools/Api";

interface ICurrencyItem {
  code: string;
  name: string;
  nameLocal: string;
  symbol: string;
  subunitValue: number;
  subunitName: string;
}

const getCurrencies = async (): Promise<ICurrencyItem[]> => {
  const response = await api.get<ICurrencyItem[]>("/currency");

  return response.data;
};

export { type ICurrencyItem, getCurrencies };
