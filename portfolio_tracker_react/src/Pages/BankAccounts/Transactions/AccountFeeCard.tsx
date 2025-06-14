import React from 'react';
import { ITransactionGroupCardProps } from './TransactionGroupCard';
import { TransactionType } from '../../../Api';

const AccountFeeCard: React.FC<ITransactionGroupCardProps> = (props) => {
  const { item, currencyCode, localeCode } = props;

  const { description, transactions } = item;

const paymentTransaction = transactions.find((tx) => tx.transactionType === TransactionType.Main);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat(localeCode || "en-US", {
      style: "currency",
      currency: currencyCode || "USD"
    }).format(value);
  };

  return (

    <div >
      {item.description && <p>{description}</p>}
      {paymentTransaction && <p><strong>Amount:</strong> {formatCurrency(paymentTransaction.price)}</p>}
    </div>
  );
};

export default AccountFeeCard;
