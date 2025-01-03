import { ITransactionItem } from "../Store/TransactionSlice";
import api from "../Tools/Api";

export const fetchTransactionsApi = async (): Promise<ITransactionItem[]> => {
  const response = await api.get<ITransactionItem[]>("/transaction/list");

  return response.data;
};
