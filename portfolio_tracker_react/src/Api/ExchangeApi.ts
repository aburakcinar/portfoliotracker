import api from "../Tools/Api";

export interface IExchangeQueryModel {
  mic: string;
  marketNameInstitutionDescription: string;
  legalEntityName: string;
  countryCode: string;
  city: string;
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
