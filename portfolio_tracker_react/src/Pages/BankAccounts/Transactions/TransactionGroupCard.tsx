import React from "react";
import { IBankAccountTransactionGroupModel, IBankAccountTransactionModel } from "../../../Api";
import { Card } from "primereact/card";
import AccountFeeCard from "./AccountFeeCard";
import BuyAssetCard from "./BuyAssetCard";
import DepositCard from "./DepositCard";
import DividendDistributionCard from "./DividendDistributionCard";
import InterestPaymentCard from "./InterestPaymentCard";
import SellAssetCard from "./SellAssetCard";
import WithdrawCard from "./WithdrawCard";

export interface ITransactionGroupCardProps {
    item: IBankAccountTransactionGroupModel;
    currencyCode: string;
    localeCode: string;
}


export const TransactionGroupCard: React.FC<ITransactionGroupCardProps> = (props) => {
    const { item, currencyCode, localeCode } = props;

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat(localeCode || "en-US", {
            style: "currency",
            currency: currencyCode || "USD"
        }).format(value);
    };

    const formatDate = (value: Date) => {
        return new Date(value).toLocaleDateString("en-GB");
    };
    switch (item.actionTypeCode) {
        case "DEPOSIT":
            return (
                <Card key={item.id} title={item.actionTypeName} className="mb-3">
                    <DepositCard
                        item={item}
                        currencyCode={currencyCode}
                        localeCode={localeCode}
                    />
                </Card>
            );
        case "WITHDRAW":
            return (
                <Card key={item.id} title={item.actionTypeName} className="mb-3">
                    <WithdrawCard
                        amount={item.transactions[0]?.price ?? 0}
                        account={item.description}
                        date={formatDate(item.executeDate)}
                        description={item.transactions[0]?.description}
                    />
                </Card>
            );
        case "BUY":
            return (
                <BuyAssetCard
                    item={item}
                    currencyCode={currencyCode}
                    localeCode={localeCode}
                />
            );
        case "SELL":
            return (
                <SellAssetCard
                    asset={item.description}
                    amount={item.transactions[0]?.quantity ?? 0}
                    price={item.transactions[0]?.price ?? 0}
                    date={formatDate(item.executeDate)}
                    tax={item.transactions.find(t => t.transactionType === 2)?.price}
                    fee={item.transactions.find(t => t.transactionType === 3)?.price}
                    description={item.transactions[0]?.description}
                />
            );
        case "DIVIDEND":
            return (
                <Card key={item.id} title={item.actionTypeName} className="mb-3">
                    <DividendDistributionCard
                        item={item}
                        currencyCode={currencyCode}
                        localeCode={localeCode}
                    />
                </Card>
            );
        case "ACCOUNT_FEE":
            return (
                <Card key={item.id} title={item.actionTypeName} className="mb-3">
                    <AccountFeeCard
                        item={item}
                        currencyCode={currencyCode}
                        localeCode={localeCode}
                    />
                </Card>
            );
        case "INTEREST":
            return (
                <Card key={item.id} title={item.actionTypeName} className="mb-3">
                    <InterestPaymentCard
                        amount={item.transactions[0]?.price ?? 0}
                        date={formatDate(item.executeDate)}
                        description={item.transactions[0]?.description}
                    />
                </Card>
            );
        default:
            return (
                <Card key={item.id} title={item.actionTypeName} className="mb-3">
                    <div>
                        <strong>{item.actionTypeName}</strong>
                        <p>{item.description}</p>
                    </div>
                </Card>
            );
    }

    // return ( {switch (item.actionTypeCode) {
    //                 case "DEPOSIT":
    //                     return (
    //                         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                             <DepositCard
    //                                 item={item}
    //                                 currencyCode={currencyCode}
    //                                 localeCode={localeCode}
    //                             />
    //                         </Card>
    //                     );
    //                 // case "WITHDRAW":
    //                 //     return (
    //                 //         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                 //             <WithdrawCard
    //                 //                 key={transaction.id}
    //                 //                 amount={transaction.price * transaction.quantity}
    //                 //                 account={item.bankAccountId}
    //                 //                 date={formatDate(item.executeDate)}
    //                 //                 description={transaction.description}
    //                 //             />
    //                 //         </Card>
    //                 //     );
    //                 case "BUY_ASSET":
    //                     return (
    //                         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                             <BuyAssetCard
    //                                 key={item.id}
    //                                 item={item}
    //                                 currencyCode={currencyCode}
    //                                 localeCode={localeCode}
    //                             />
    //                         </Card>
    //                     );
    //                 // case "SELL_ASSET":
    //                 //     return (
    //                 //         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                 //             <SellAssetCard
    //                 //                 key={transaction.id}
    //                 //                 asset={transaction.description || "Asset"}
    //                 //                 amount={transaction.quantity}
    //                 //                 price={transaction.price}
    //                 //                 date={formatDate(item.executeDate)}
    //                 //                 tax={transaction.tax}
    //                 //                 fee={transaction.fee}
    //                 //                 description={transaction.description}
    //                 //             />
    //                 //         </Card>
    //                 //     );
    //                 case "DIVIDEND_DISTRIBUTION":
    //                     return (
    //                         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                             <DividendDistributionCard
    //                                 item={item}
    //                                 currencyCode={currencyCode}
    //                                 localeCode={localeCode}
    //                             />
    //                         </Card>
    //                     );
    //                 // case "INTEREST_PAYMENT":
    //                 //     return (
    //                 //         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                 //             <InterestPaymentCard
    //                 //                 key={transaction.id}
    //                 //                 amount={transaction.price * transaction.quantity}
    //                 //                 date={formatDate(item.executeDate)}
    //                 //                 description={transaction.description}
    //                 //             />
    //                 //         </Card>
    //                 //     );
    //                 case "ACCOUNT_FEE":
    //                     return (
    //                         <Card key={item.id} title={item.actionTypeName} className="mb-3">
    //                             <AccountFeeCard
    //                                 item={item}
    //                                 currencyCode={currencyCode}
    //                                 localeCode={localeCode}
    //                             />
    //                         </Card>
    //                     );
                  
    //             }
    //         }
    //     </>
    // );
}
