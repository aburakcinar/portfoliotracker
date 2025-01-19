import {
  IHoldingDetailModel,
  IPortfolioHoldingModel,
  IPortfolioModel,
} from "../Store/Models";
import { IStockBuyRequest } from "../Store/Thunks/PortfolioThunks";
import api from "../Tools/Api";

const createPortfolio = async (
  portfolioName: string,
  currencyCode: string
): Promise<boolean> => {
  const response = await api.post<{
    portfolioName: string;
    currencyCode: string;
  }>("/portfolio/create", {
    portfolioName,
    currencyCode,
  });

  return response.status === 200;
};

const buyStock = async (payload: IStockBuyRequest): Promise<boolean> => {
  return await api.post<IStockBuyRequest, boolean>("/portfolio/buy", payload);
};

const reserveStockOnPortfolioApi = async (
  portfolioId: string,
  stockSymbol: string
): Promise<boolean> => {
  return await api.post<void, boolean>(
    `/portfolio/holdings/${portfolioId}/reserve/${stockSymbol}`
  );
};

const listHoldingsByPortfolio = async (
  portfolioId: string
): Promise<IPortfolioHoldingModel[]> => {
  const response = await api.get<IPortfolioHoldingModel[]>(
    `/portfolio/holdings/${portfolioId}`
  );

  return response.data;
};

const listHoldingDetails = async (
  portfolioId: string,
  stockSymbol: string
): Promise<IHoldingDetailModel[]> => {
  const response = await api.get<IHoldingDetailModel[]>(
    `/portfolio/holdings/${portfolioId}/details/${stockSymbol}`
  );

  return response.data;
};

export {
  createPortfolio,
  buyStock,
  listHoldingsByPortfolio,
  listHoldingDetails,
  reserveStockOnPortfolioApi,
};
