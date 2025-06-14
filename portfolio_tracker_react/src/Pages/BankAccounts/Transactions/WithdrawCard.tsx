import React from 'react';

export interface WithdrawCardProps {
  amount: number;
  account: string;
  date: string;
  description?: string;
}

const WithdrawCard: React.FC<WithdrawCardProps> = ({ amount, account, date, description }) => (
  <div className="transaction-card withdraw-card">
    <h3>Withdraw</h3>
    <p><strong>Account:</strong> {account}</p>
    <p><strong>Amount:</strong> {amount}</p>
    <p><strong>Date:</strong> {date}</p>
    {description && <p><strong>Description:</strong> {description}</p>}
  </div>
);

export default WithdrawCard;
