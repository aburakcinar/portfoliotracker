import api from "../Tools/Api";

export interface IExchangeQueryModel {
  code: string;
  mic: string;
  marketNameInstitutionDescription: string;
  legalEntityName: string;
  countryCode: string;
  city: string;
  currencyCode: string;
}

export const searchExchangesApi = async (
  searchText: string,
  limit = 50
): Promise<IExchangeQueryModel[]> => {
  const response = await api.get<IExchangeQueryModel[]>(
    `/exchange/query?searchText=${searchText}&limit=${limit}`
  );

  return response.data;
};

export const getExchangeApi = async (
  mic: string
): Promise<IExchangeQueryModel | null> => {
  const response = await api.get<IExchangeQueryModel | null>(
    `/exchange/get/${mic}`
  );

  return response.data;
};
