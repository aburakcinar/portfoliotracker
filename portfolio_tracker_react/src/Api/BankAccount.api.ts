import { ImportItem } from "../Pages/BankAccounts";
import api from "../Tools/Api";

export interface ICreateBankAccountCommand {
  name: string;
  bankName: string;
  accountHolder: string;
  description: string;
  iban: string;
  currencyCode: string;
  localeCode: string;
  openDate: Date;
}

export const createBankAccountApi = async (
  command: ICreateBankAccountCommand
): Promise<boolean> => {
  const response = await api.post<boolean>("/bankaccount", command);

  return response.data;
};

export interface IBankAccountModel {
  id: string;
  name: string;
  bankName: string;
  accountHolder: string;
  description: string;
  iban: string;
  currencyCode: string;
  localeCode: string;
  openDate: Date;
  created: Date;
}

export const fetchBankAccountsApi = async (): Promise<IBankAccountModel[]> => {
  const response = await api.get<IBankAccountModel[]>("/bankaccount/");

  return response.data;
};

export const getBankAccountApi = async (
  bankAccountId: string
): Promise<IBankAccountModel | null> => {
  const response = await api.get<IBankAccountModel | null>(
    `/bankaccount/get/${bankAccountId}`
  );

  return response.data;
};

interface IImportBankAccountsCommand {
  items: ImportItem[];
  override: boolean;
}

export const importBankAccountsApi = async (items: ImportItem[]): Promise<boolean> => {

  const command = {
    items,
    override: true,
  } satisfies IImportBankAccountsCommand;

  const response = await api.post<boolean>("/bankaccount/import", command);

  return response.data;
};

export const deleteBankAccountApi = async (
  bankAccountId: string
): Promise<boolean> => {
  const response = await api.delete<boolean>(
    `/bankaccount/${bankAccountId}`
  );

  return response.data;
};