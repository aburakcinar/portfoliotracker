import {
  AutoComplete,
  AutoCompleteChangeEvent,
  AutoCompleteCompleteEvent,
} from "primereact/autocomplete";
import React, { useState } from "react";
import { ICurrencyItem } from "../Api";

export interface ICurrencyPickerProps {
  value?: string;
  onChange: (event: any) => void;
}

export const CurrencyPicker: React.FC<ICurrencyPickerProps> = (props) => {
  const { value, onChange } = props;
  const [currencies, setCurrencies] = useState<ICurrencyItem[]>([]);
  const [filteredCurrencies, setFilteredCurrencies] = useState<ICurrencyItem[]>(
    []
  );
  const [innerValue, setInnerValue] = useState<ICurrencyItem[]>([]);

  console.log(filteredCurrencies);

  const searchCurrencyHandler = async ($event: AutoCompleteCompleteEvent) => {
    const searchText = $event.query.toLowerCase().trim();

    console.log(searchText);

    if (searchText.length === 0) {
      setFilteredCurrencies([]);
    } else {
      const result = currencies.filter(
        (x) =>
          x.code.toLowerCase().includes(searchText) ||
          x.name.toLowerCase().includes(searchText) ||
          x.nameLocal.toLowerCase().includes(searchText)
      );

      setFilteredCurrencies(
        result.map((x) => {
          return {
            ...x,
            placeholder: `${x.code} - ${x.name}`,
          };
        })
      );
    }
  };

  const currencyItemTemplate = (item: ICurrencyItem) => {
    return (
      <div className="flex text-sm">
        {item.code} - {item.name}
      </div>
    );
  };

  const onChangeHandler = ($event: AutoCompleteChangeEvent) => {
    console.log($event.value);
  };

  return (
    <AutoComplete
      field="placeholder"
      value={innerValue}
      onChange={onChangeHandler}
      suggestions={filteredCurrencies}
      completeMethod={searchCurrencyHandler}
      itemTemplate={currencyItemTemplate}
      multiple
      selectionLimit={1}
    />
  );
};
