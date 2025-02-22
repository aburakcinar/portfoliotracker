import { ITransactionItem } from "../Store/Transaction.slice";
import api from "../Tools/Api";

export const fetchTransactionsApi = async (): Promise<ITransactionItem[]> => {
  const response = await api.get<ITransactionItem[]>("/transaction/list");

  return response.data;
};

export enum TransactionActionTypeCategory {
  Unknown = 0,
  Outgoing = 1,
  Tax = 2,
  Fee = 3,
  Incoming = 4,
}

export interface ITransactionActionType {
  code: string;
  name: string;
  description: string;
  category: TransactionActionTypeCategory;
}

export const fetchTransactionActionTypesApi = async (): Promise<
  ITransactionActionType[]
> => {
  const response = await api.get<ITransactionActionType[]>(
    "/transaction/actiontypes"
  );

  return response.data;
};
