import api from "./Api";
import { IPortfolioModel } from "../Store";

export interface ICreatePortfolioRequest {
  name: string;
  description: string;
  bankAccountId: string;
}

export const createPortfolioApi = async (
  request: ICreatePortfolioRequest
): Promise<boolean> => {
  const response = await api.post<boolean>("/portfolio", request);

  return response.data;
};

export interface IUpdatePortfolioRequest {
  portfolioId: string;
  name: string;
  description: string;
  bankAccountId: string;
}

export const updatePortfolioApi = async (
  request: IUpdatePortfolioRequest
): Promise<boolean> => {
  const response = await api.put<boolean>("/portfolio", request);

  return response.data;
};

export const listPortfoliosApi = async (): Promise<IPortfolioModel[]> => {
  const response = await api.get<IPortfolioModel[]>("/portfolio");

  return response.data;
};

export const getPortfolioApi = async (
  portfolioId: string
): Promise<IPortfolioModel | null> => {
  const response = await api.get<IPortfolioModel | null>(
    `/portfolio/${portfolioId}`
  );

  return response.data;
};

export interface IPortfolioTotalPositionResultModel {
  portfolioId: string;
  totalPosition: number;
  totalCost: number;
  totalExpenses: number;
  currencyCode: string;
  currencySymbol: string;
}

export const getPortfolioTotalApi = async (
  portfolioId: string
): Promise<IPortfolioTotalPositionResultModel | null> => {
  const response = await api.get<IPortfolioTotalPositionResultModel | null>(
    `portfolio/total/${portfolioId}`
  );

  return response.data;
};
