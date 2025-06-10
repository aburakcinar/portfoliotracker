import api from "./Api";

export enum InOut {
  In = 1,
  Out = 2,
}

export enum TransactionType{
  Main = 1,
  Tax = 2,
  Fee = 3
}

// export interface ITransactionCreateModel {
//   price: number;
//   quantity: number;
//   inOut: InOut;
//   actionTypeCode: string;
// }

// export interface IAddTransactionCommand {
//   bankAccountId: string;
//   operationDate: Date;
//   transactions: ITransactionCreateModel[];
// }

// export const addTransactionApi = async (
//   command: IAddTransactionCommand
// ): Promise<boolean> => {
//   var response = await api.post<boolean>("transaction", command);

//   return response.data;
// };

export interface IBankAccountTransactionModel {
  id: string;
  price: number;
  quantity: number;
  inOut: InOut;
  transactionType: TransactionType;
  description: string;
}

export interface IBankAccountTransactionGroupModel {
  id: string;
  bankAccountId: string;
  referenceNo: string;
  transactions: IBankAccountTransactionModel[];
  description: string;
  actionTypeCode: string;
  actionTypeName: string;
  executeDate: Date;
}

export const listBankAccountTransactionsApi = async (
  bankAccountId: string
): Promise<IBankAccountTransactionGroupModel[]> => {
  var response = await api.get<IBankAccountTransactionGroupModel[]>(
    `transaction/listbybankaccount/${bankAccountId}`
  );

  return response.data;
};
