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

interface IPortfolioItem {
  id: string;
  name: string;
  description: string;
  currencyCode: string;
  currencyName: string;
  currencySymbol: string;
}

const listPortfolios = async (): Promise<IPortfolioItem[]> => {
  const response = await api.get<IPortfolioItem[]>("/portfolio/list");

  return response.data;
};

export { type IPortfolioItem, createPortfolio, listPortfolios };
