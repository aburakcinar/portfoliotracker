import api from "./Api";

export interface ILocaleQueryModel {
  localeCode: string;
  countryName: string;
  countryCode: string;
  currencyCode: string;
  currencyName: string;
  currencySymbol: string;
}

export const searchLocalesApi = async (
  searchText: string,
  limit = 50
): Promise<ILocaleQueryModel[]> => {
  const response = await api.get<ILocaleQueryModel[]>(
    `/locale/search?searchText=${searchText}&limit=${limit}`
  );

  return response.data;
};
