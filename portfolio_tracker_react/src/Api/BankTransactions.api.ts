import api from "../Tools/Api";

export enum InOut {
  In = 1,
  Out = 2,
}

export interface ITransactionCreateModel {
  price: number;
  quantity: number;
  inOut: InOut;
  actionTypeCode: string;
}

export interface IAddTransactionCommand {
  bankAccountId: string;
  operationDate: Date;
  transactions: ITransactionCreateModel[];
}

export const addTransactionApi = async (
  command: IAddTransactionCommand
): Promise<boolean> => {
  var response = await api.post<boolean>("transaction", command);

  return response.data;
};

export interface IBankTransactionModel {
  id: string;
  price: number;
  quantity: number;
  inOut: InOut;
  actionTypeCode: string;
}

export interface IBankTransactionGroupModel {
  id: string;
  bankAccountId: string;
  operationDate: Date;
  amount: number;
  balance: number;
  transactions: IBankTransactionModel[];
  description: string;
}

export const listBankAccountTransactionsApi = async (
  bankAccountId: string
): Promise<IBankTransactionGroupModel[]> => {
  var response = await api.get<IBankTransactionGroupModel[]>(
    `transaction/listbybankaccount/${bankAccountId}`
  );

  return response.data;
};
