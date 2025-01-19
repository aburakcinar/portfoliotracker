import { Card } from "primereact/card";
import React from "react";
import { ICurrencyItem } from "../../Api";
import { Dropdown } from "primereact/dropdown";

const currencies: ICurrencyItem[] = [
  { code: "TRY", name: "Turkish Lira", symbol: "TL" },
  { code: "EUR", name: "Euro", symbol: "EUR" },
  { code: "USD", name: "United States Dollar", symbol: "USD" },
];

export const ExampleComboboxes: React.FC = () => {
  return (
    <>
      <h3 className="text-2xl text-green m-10 my-4">Comboboxes</h3>

      <Card className="m-10">
        <Dropdown
          className="h-10"
          options={currencies}
          optionLabel="code"
          placeholder="Select a currency"
        />
      </Card>
    </>
  );
};
