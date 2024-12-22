import { Dropdown } from "primereact/dropdown";
import { ICurrencyItem } from "../Api/CurrencyApi";
import { DarkModeToggle } from "../Controls/DarkModeToggle";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { AddInvestmentControl } from "../Popups/AddInvestmentControl";

const currencies: ICurrencyItem[] = [
  { code: "TRY", name: "Turkish Lira", symbol: "TL" },
  { code: "EUR", name: "Euro", symbol: "EUR" },
  { code: "USD", name: "United States Dollar", symbol: "USD" },
];

export const ExampleControls: React.FC = () => {
  return (
    <div>
      <h3 className="text-5xl text-green p-4">Example Controls</h3>

      <h3 className="text-2xl text-green m-10 my-4">Add New Investment Form</h3>
      <Card className="m-10">
        <AddInvestmentControl currency="TRY" portfolioId="examplePortfolio" />
      </Card>

      <h3 className="text-2xl text-green m-10 my-4">Dark Mode Toggle</h3>

      <Card className="m-10">
        <DarkModeToggle />
      </Card>

      <h3 className="text-2xl text-green m-10 my-4">Comboboxes</h3>

      <Card className="m-10">
        <Dropdown
          className="h-10"
          options={currencies}
          optionLabel="code"
          placeholder="Select a currency"
        />
      </Card>

      <h3 className="text-2xl text-green m-10 my-4">TextField</h3>
      <Card className="m-10">
        <InputText className="w-20 h-10" />
      </Card>
    </div>
  );
};
