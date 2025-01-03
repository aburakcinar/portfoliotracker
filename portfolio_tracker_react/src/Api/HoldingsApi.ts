import api from "../Tools/Api";

export interface IUpdateHoldingRequest {
  portfolioId: string;
  stockSymbol: string;
  holdingId: string;
  quantity: number;
  price: number;
  expenses: number;
  executeDate?: Date;
}

export interface IReportSellRequest {
  price: number;
  quantity: number;
  expenses: number;
  executeDate: Date;
}

export const updateHoldingApi = async (
  payload: IUpdateHoldingRequest
): Promise<boolean> => {
  const response = await api.put("/holding/update", payload);

  return response.data;
};

export const deleteHoldingApi = async (holdingId: string): Promise<boolean> => {
  const response = await api.delete(`/holding/delete/${holdingId}`);

  return response.data;
};

export const reportSellHoldingApi = async (
  holdingId: string,
  payload: IReportSellRequest
): Promise<boolean> => {
  const response = await api.put<boolean>(
    `/holding/reportsell/${holdingId}`,
    payload
  );

  return response.data;
};
