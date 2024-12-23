import { IPortfolioModel } from "../Store/Models";
import { IStockBuyRequest } from "../Store/PortfolioSlice/PortfolioThunks";
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

const listPortfolios = async (): Promise<IPortfolioModel[]> => {
  const response = await api.get<IPortfolioModel[]>("/portfolio/list");

  return response.data;
};

const buyStock = async (payload: IStockBuyRequest): Promise<boolean> => {
  return await api.post<IStockBuyRequest, boolean>("/portfolio/buy", payload);
};

export { createPortfolio, listPortfolios, buyStock };
