import { ITransactionItem } from "../Store/Transaction.slice";
import api from "./Api";
import { IBankAccountTransactionGroupModel } from "./BankTransactions.api";

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

export const listTransactionsByBankAccountIdApi = async (bankAccountId: string): Promise<ITransactionItem[]> => {
  const response = await api.get<ITransactionItem[]>(`/transaction/listbybankaccount/${bankAccountId}`);

  return response.data;
};

export interface IDraftTransactionItem extends Omit<ITransactionItem, 'portfolioId'> {
  portfolioId?: string;
}

export interface ImportTransactionsFromFileCommand {
  bankAccountId: string;
  file: File;
}

export const draftImportTransactionsFromFileApi = async (command: ImportTransactionsFromFileCommand): Promise<IBankAccountTransactionGroupModel[]> => {
  const formData = new FormData();
  formData.append('bankAccountId', command.bankAccountId);
  formData.append('file', command.file);

  const response = await api.post<IBankAccountTransactionGroupModel[]>('/import/draft-import-transactions', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });

  console.log(response.data);

  return response.data;
};