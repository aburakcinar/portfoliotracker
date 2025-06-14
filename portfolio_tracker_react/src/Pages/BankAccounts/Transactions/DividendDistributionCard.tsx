import React from 'react';
import { ITransactionGroupCardProps } from './TransactionGroupCard';
import { TransactionType } from '../../../Api';

const DividendDistributionCard: React.FC<ITransactionGroupCardProps> = (props) => {

  const { item, currencyCode, localeCode } = props;
  const { transactions, description } = item;
  const main = transactions.find(x => x.transactionType === TransactionType.Main);


  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat(localeCode || "en-US", {
      style: "currency",
      currency: currencyCode || "USD"
    }).format(value);
  };

  return (
    <>
      <p><strong>Asset:</strong> {description}</p>
      <p><strong>Amount:</strong> {formatCurrency((main?.price ?? 0) * (main?.quantity ?? 0))}</p>
    </>
  );
}

export default DividendDistributionCard;
