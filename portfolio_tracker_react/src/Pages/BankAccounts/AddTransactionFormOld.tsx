import React, { useState } from "react";
import {
  ITransactionActionType,
  TransactionActionTypeCategory,
} from "../../Api/Transaction.api";
import { classNames } from "primereact/utils";
import { Dropdown } from "primereact/dropdown";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import { DataTable } from "primereact/datatable";
import {
  addTransaction,
  IBankAccountTransactionCreateModel,
} from "../../Store/Transaction.slice";
import { Column } from "primereact/column";
import { IAddTransactionCommand, InOut } from "../../Api/BankTransactions.api";
import { InputNumber } from "primereact/inputnumber";
import { TrashIcon, PlusIcon } from "@heroicons/react/24/outline";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { InputText } from "primereact/inputtext";
import { Card } from "primereact/card";
import { Tooltip } from "primereact/tooltip";

export interface IAddTransactionFormOldProps {
  bankAccountId: string;
  currencyCode: string;
}

export const AddTransactionFormOld: React.FC<IAddTransactionFormOldProps> = (props) => {
  const actionTypes = useAppSelector((x) => x.transactions.actionTypes);
  const actionTypeCategories = useAppSelector((x) => x.transactions.actionTypeCategories);
  const [operationDate, setOperationDate] = useState<Date>(new Date());
  const { bankAccountId, currencyCode } = props;
  const dispatch = useAppDispatch();
  const [transactions, setTransactions] = useState<IBankAccountTransactionCreateModel[]>([]);
  const [activeCategory, setActiveCategory] = useState<TransactionActionTypeCategory | null>(null);
  const [newTransaction, setNewTransaction] = useState<{
    price: number;
    quantity: number;
    actionTypeCode: string;
    description: string;
  }>({ price: 0, quantity: 1, actionTypeCode: "", description: "" });

  const renderCategoryTabs = () => {
    return (
      <div className="flex gap-2 mb-4">
        {actionTypeCategories.map((item) => {
          const isActive = activeCategory === item.category;
          return (
            <button
              key={item.label}
              onClick={() => setActiveCategory(item.category)}
              className={classNames(
                "px-4 py-2 rounded-t-lg font-medium transition-colors",
                isActive
                  ? "bg-green text-white border-b-2 border-green-700"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200"
              )}
            >
              {item.label}
            </button>
          );
        })}
      </div>
    );
  };

  const getCategory = (category: TransactionActionTypeCategory): string => {
    return actionTypeCategories.find((x) => x.category === category)?.label ?? "";
  };

  const getInOut = (category: TransactionActionTypeCategory): InOut => {
    return category === TransactionActionTypeCategory.Incoming ? InOut.In : InOut.Out;
  };

  const addNewTransaction = () => {
    if (!activeCategory || !newTransaction.actionTypeCode || newTransaction.price <= 0) {
      return; // Validation failed
    }

    setTransactions((prev) => [
      ...prev,
      {
        index: Math.max(...prev.map((x) => x.index), -1) + 1,
        price: newTransaction.price,
        quantity: newTransaction.quantity,
        inOut: getInOut(activeCategory),
        actionTypeCode: newTransaction.actionTypeCode,
        actionTypeCategory: activeCategory,
        description: newTransaction.description,
      },
    ]);

    // Reset the form
    setNewTransaction({ price: 0, quantity: 1, actionTypeCode: "", description: "" });
  };

  const renderTransactionForm = () => {
    if (!activeCategory) return null;

    const filteredActionTypes = actionTypes.filter((x) => x.category === activeCategory);
    const categoryName = getCategory(activeCategory);
    const isIncome = activeCategory === TransactionActionTypeCategory.Incoming;
    const amountColor = isIncome ? "text-emerald-500" : "text-red-500";

    return (
      <Card className="mb-4 shadow-sm" title="Add Transaction">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label htmlFor="actionType" className="block text-sm font-medium text-gray-700 mb-1">
              {categoryName} Type
            </label>
            <Dropdown
              id="actionType"
              options={filteredActionTypes}
              value={newTransaction.actionTypeCode}
              optionValue="code"
              optionLabel="name"
              onChange={(e) => setNewTransaction({...newTransaction, actionTypeCode: e.value})}
              placeholder="Select type"
              className="w-full"
            />
          </div>
          
          <div>
            <label htmlFor="amount" className="block text-sm font-medium text-gray-700 mb-1">
              Amount
            </label>
            <InputNumber
              id="amount"
              value={newTransaction.price}
              onValueChange={(e) => setNewTransaction({...newTransaction, price: e.value || 0})}
              mode="currency"
              currency={currencyCode}
              className={`w-full ${amountColor}`}
              placeholder="0.00"
              min={0}
            />
          </div>
          
          <div>
            <label htmlFor="quantity" className="block text-sm font-medium text-gray-700 mb-1">
              Quantity
            </label>
            <InputNumber
              id="quantity"
              value={newTransaction.quantity}
              onValueChange={(e) => setNewTransaction({...newTransaction, quantity: e.value || 1})}
              mode="decimal"
              min={1}
              className="w-full"
            />
          </div>
          
          <div>
            <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-1">
              Description
            </label>
            <InputText
              id="description"
              value={newTransaction.description}
              onChange={(e) => setNewTransaction({...newTransaction, description: e.target.value})}
              className="w-full"
              placeholder="Optional description"
            />
          </div>
        </div>
        
        <div className="flex justify-end mt-4">
          <Button
            icon={<PlusIcon className="h-5 w-5 mr-2" />}
            label="Add Transaction"
            className="bg-green hover:bg-green-600"
            onClick={addNewTransaction}
            disabled={!newTransaction.actionTypeCode || newTransaction.price <= 0}
          />
        </div>
      </Card>
    );
  };

  const removeTransaction = (index: number) => {
    setTransactions((prev) => prev.filter((x) => x.index !== index));
  };

  const getTransactionAmount = (item: IBankAccountTransactionCreateModel) => {
    return (item.inOut === InOut.Out ? -1 : 1) * item.price * item.quantity;
  };

  const amountTemplate = (item: IBankAccountTransactionCreateModel) => {
    const amount = getTransactionAmount(item);
    const className = amount >= 0 ? "text-emerald-500" : "text-red-500";
    
    return <span className={className}>{amount.toFixed(2)} {currencyCode}</span>;
  };

  const calculateTotal = () => {
    return transactions.reduce(
      (prev, next) => prev + getTransactionAmount(next),
      0
    );
  };

  const onSaveButton = () => {
    if (transactions.length === 0) return;
    
    const command = {
      bankAccountId,
      operationDate,
      transactions,
    } satisfies IAddTransactionCommand;

    dispatch(addTransaction(command));
    
    // Clear form after saving
    setTransactions([]);
    setActiveCategory(null);
  };

  const actionTypeTemplate = (item: IBankAccountTransactionCreateModel) => {
    const type = actionTypes.find(t => t.code === item.actionTypeCode);
    return <span>{type?.name || item.actionTypeCode}</span>;
  };

  const actionTemplate = (item: IBankAccountTransactionCreateModel) => {
    return (
      <Button
        icon={<TrashIcon className="h-4 w-4" />}
        className="p-button-rounded p-button-danger p-button-text"
        onClick={() => removeTransaction(item.index)}
        tooltip="Remove"
      />
    );
  };

  return (
    <div className="flex flex-col p-4 bg-white rounded-lg shadow-sm">
      <div className="flex flex-col md:flex-row gap-4 mb-4">
        <div className="w-full md:w-1/3">
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Transaction Date
          </label>
          <Calendar
            value={operationDate}
            onChange={(e) => setOperationDate(e.value ?? new Date())}
            dateFormat="dd/mm/yy"
            showIcon
            className="w-full"
          />
        </div>
      </div>
      
      {renderCategoryTabs()}
      {renderTransactionForm()}
      
      {transactions.length > 0 && (
        <>
          <h3 className="text-lg font-medium mb-2">Transaction Summary</h3>
          <DataTable
            value={transactions}
            className="mb-4"
            rowGroupMode="subheader"
            groupRowsBy="actionTypeCategory"
            sortMode="single"
            sortField="actionTypeCategory"
            sortOrder={1}
            rowGroupHeaderTemplate={(data) => (
              <div className="font-bold text-lg py-2">{getCategory(data.actionTypeCategory)}</div>
            )}
            size="small"
          >
            <Column field="description" header="Description" />
            <Column field="actionTypeCode" header="Type" body={actionTypeTemplate} />
            <Column field="quantity" header="Quantity" />
            <Column field="price" header="Price" body={(item) => `${item.price.toFixed(2)} ${currencyCode}`} />
            <Column field="amount" header="Amount" body={amountTemplate} />
            <Column body={actionTemplate} style={{ width: '4rem' }} />
          </DataTable>
          
          <div className="flex justify-between items-center border-t pt-4">
            <div className="text-lg font-semibold">
              Total: <span className={calculateTotal() >= 0 ? "text-emerald-500" : "text-red-500"}>
                {calculateTotal().toFixed(2)} {currencyCode}
              </span>
            </div>
            
            <Button
              label="Save Transactions"
              icon="pi pi-check"
              className="bg-green hover:bg-green-600"
              onClick={onSaveButton}
              disabled={transactions.length === 0}
            />
          </div>
        </>
      )}
      
      <Tooltip target=".p-button-rounded" />
    </div>
  );
};
