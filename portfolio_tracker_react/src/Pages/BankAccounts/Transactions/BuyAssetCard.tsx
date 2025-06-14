import React from 'react';
import { ITransactionGroupCardProps } from './TransactionGroupCard';
import { TransactionType } from '../../../Api';
import { Card } from 'primereact/card';
import { Divider } from 'primereact/divider';
import { Tag } from 'primereact/tag';

const BuyAssetCard: React.FC<ITransactionGroupCardProps> = (props) => {
  const { item, currencyCode, localeCode } = props;
  const { transactions, description } = item;
  const main = transactions.find(x => x.transactionType === TransactionType.Main);
  const tax = transactions.find(x => x.transactionType === TransactionType.Tax);
  const fee = transactions.find(x => x.transactionType === TransactionType.Fee);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat(localeCode || "en-US", {
      style: "currency",
      currency: currencyCode || "USD"
    }).format(value);
  };

  const formatDate = (value: Date) => {
    return new Date(value).toLocaleDateString("en-GB");
  };

  // Calculate total cost including tax and fee
  const mainCost = main ? main.price * main.quantity : 0;
  const taxCost = tax ? tax.price * tax.quantity : 0;
  const feeCost = fee ? fee.price * fee.quantity : 0;
  const totalCost = mainCost + taxCost + feeCost;

  const header = (
    <div className="flex items-center justify-between">
      <div className="text-xl font-bold">{description || "Asset"}</div>
      <Tag value="Buy" severity="success" />
    </div>
  );

  const footer = (
    <div className="flex justify-end mt-3">
      <div className="text-lg font-bold">
        Total: {formatCurrency(totalCost)}
      </div>
    </div>
  );

  return (
    <Card header={header} footer={footer} className="mb-3 shadow-sm">
      <div className="grid grid-cols-2 gap-4">
        <div className="col-span-2 md:col-span-1">
          <div className="mb-2">
            <span className="text-sm text-gray-500">Quantity</span>
            <div className="text-lg font-medium">{main?.quantity}</div>
          </div>
        </div>
        <div className="col-span-2 md:col-span-1">
          <div className="mb-2">
            <span className="text-sm text-gray-500">Price per Unit</span>
            <div className="text-lg font-medium">{formatCurrency(main?.price || 0)}</div>
          </div>
        </div>
       
      </div>

      {(tax || fee) && (
        <>
          <Divider className="my-3" />
          <div className="grid grid-cols-2 gap-4">
            {tax && (
              <div className="col-span-2 md:col-span-1">
                <div className="mb-2">
                  <span className="text-sm text-gray-500">Tax</span>
                  <div className="text-lg font-medium">{formatCurrency(tax.price * tax.quantity)}</div>
                </div>
              </div>
            )}
            {fee && (
              <div className="col-span-2 md:col-span-1">
                <div className="mb-2">
                  <span className="text-sm text-gray-500">Fee</span>
                  <div className="text-lg font-medium">{formatCurrency(fee.price * fee.quantity)}</div>
                </div>
              </div>
            )}
          </div>
        </>
      )}
    </Card>
  );
};

export default BuyAssetCard;
