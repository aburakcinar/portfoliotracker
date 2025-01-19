export interface IPortfolioModel {
  id: string;
  name: string;
  description: string;
  isDefault: boolean;
}

export interface IPortfolioHoldingModel {
  stockSymbol: string;
  quantity: number;
  averagePrice: number;
  totalExpenses: number;
}

export interface IHoldingPurchaseModel {
  id: string;
  quantity: number;
  price: number;
  expenses: number;
  executeDate: Date;
}

export interface IHoldingDetailModel {
  id: string;
  quantity: number;
  price: number;
  expenses: number;
  executeDate: Date;
}
