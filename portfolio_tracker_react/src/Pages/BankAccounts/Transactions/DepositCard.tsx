import React from 'react';
import { ITransactionGroupCardProps } from './TransactionGroupCard';
import { TransactionType } from '../../../Api';


const DepositCard: React.FC<ITransactionGroupCardProps> = (props) => {

  const { item, currencyCode, localeCode } = props;


  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat(localeCode || "en-US", {
      style: "currency",
      currency: currencyCode || "USD"
    }).format(value);
  };

  const paymentTransaction = item.transactions.find((tx) => tx.transactionType === TransactionType.Main);

  return (
    <>
      {paymentTransaction && <p><strong>Amount:</strong> {formatCurrency(paymentTransaction?.price)}</p>}
    </>
  );
}

export default DepositCard;
