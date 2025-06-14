import React from 'react';

export interface SellAssetCardProps {
  asset: string;
  amount: number;
  price: number;
  date: string;
  tax?: number;
  fee?: number;
  description?: string;
}

const SellAssetCard: React.FC<SellAssetCardProps> = ({ asset, amount, price, date, tax, fee, description }) => (
  <div className="transaction-card sell-asset-card">
    <h3>Sell Asset</h3>
    <p><strong>Asset:</strong> {asset}</p>
    <p><strong>Amount:</strong> {amount}</p>
    <p><strong>Price:</strong> {price}</p>
    <p><strong>Date:</strong> {date}</p>
    {tax !== undefined && <p><strong>Tax:</strong> {tax}</p>}
    {fee !== undefined && <p><strong>Fee:</strong> {fee}</p>}
    {description && <p><strong>Description:</strong> {description}</p>}
  </div>
);

export default SellAssetCard;
