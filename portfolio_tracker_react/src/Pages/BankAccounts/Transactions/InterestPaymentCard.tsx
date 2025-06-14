import React from 'react';

export interface InterestPaymentCardProps {
  amount: number;
  date: string;
  description?: string;
}

const InterestPaymentCard: React.FC<InterestPaymentCardProps> = ({ amount, date, description }) => (
  <div className="transaction-card interest-payment-card">
    <h3>Interest Payment</h3>
    <p><strong>Amount:</strong> {amount}</p>
    <p><strong>Date:</strong> {date}</p>
    {description && <p><strong>Description:</strong> {description}</p>}
  </div>
);

export default InterestPaymentCard;
