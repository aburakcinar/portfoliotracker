import React from "react";
import { createRoot } from "react-dom/client";
import Home from "./Home";
import { BrowserRouter, Route, Routes } from "react-router";

import { PrimeReactProvider } from "primereact/api";

import "./tailwind.css";
import { Layout } from "./Layout";
import Transactions from "./Pages/Transactions";
import { appDesignSystem } from "./Styles/AppDesignSystem";
import { ExampleControls } from "./Pages/ExampleControls";
import { Provider } from "react-redux";
import { store } from "./Store/RootState";
import Stocks from "./Pages/Stocks";
import { ExampleAddInvestmentControl } from "./Pages/ExampleControls/ExampleAddInvestmentControl";
import { ExampleAddUpdateHoldingForm } from "./Pages/ExampleControls/ExampleAddUpdateHoldingForm";
import { ExampleDarkModeToggle } from "./Pages/ExampleControls/ExampleDarkModeToggle";
import { ExampleComboboxes } from "./Pages/ExampleControls/ExampleComboboxes";
import { ExampleInputText } from "./Pages/ExampleControls/ExampleInputText";
import { Buttons } from "./Pages/ExampleControls/Buttons";
import AssetsStocks from "./Pages/Assets/AssetsStocks";
import { AssetsLayout } from "./Pages/Assets/AssetsLayout";
import AssetsSummary from "./Pages/Assets/AssetsSummary";
import { AssetViewForm } from "./Pages/Assets/AssetViewForm";
import AssetsEtfs from "./Pages/Assets/AssetsEtfs";
import AssetsCommodities from "./Pages/Assets/AssetsCommodities";
import {
  PortfolioList,
  PortfolioLayout,
  PortfolioItem,
} from "./Pages/Portfolio";
import { BankAccountList } from "./Pages/BankAccounts/BankAccountList";
import { BankAccountLayout } from "./Pages/BankAccounts/BankAccountLayout";
import { BankAccountCreateForm } from "./Pages/BankAccounts/BankAccountCreateForm";

const rootElem = document.getElementById("root");
const root = createRoot(rootElem!);
root.render(
  <Provider store={store}>
    <PrimeReactProvider value={{ unstyled: true, pt: appDesignSystem }}>
      <BrowserRouter>
        <Routes>
          <Route element={<Layout />}>
            <Route index element={<Home />} />

            <Route path="portfolios" element={<PortfolioLayout />}>
              <Route index element={<PortfolioList />} />
              <Route path=":portfolioId" element={<PortfolioItem />} />
            </Route>

            <Route path="assets" element={<AssetsLayout />}>
              <Route index element={<AssetsSummary />} />
              <Route path="stocks" element={<AssetsStocks />} />
              <Route path="stocks/:id" element={<AssetViewForm />} />
              <Route path="etfs" element={<AssetsEtfs />} />
              <Route path="commodities" element={<AssetsCommodities />} />
            </Route>

            <Route path="bankaccounts" element={<BankAccountLayout />}>
              <Route index element={<BankAccountList />} />
              <Route path="new" element={<BankAccountCreateForm />} />
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </PrimeReactProvider>
  </Provider>
);

/*
   <Route path="portfolio" element={<Portfolio />} /> 
             <Route path="transactions" element={<Transactions />} /> 
             <Route path="controls" element={<ExampleControls />}>
              <Route
                index
                path="addinvestmentcontrol"
                element={<ExampleAddInvestmentControl />}
              />
              <Route
                path="addupdateholdingform"
                element={<ExampleAddUpdateHoldingForm />}
              />
              <Route
                path="darkmodetoggle"
                element={<ExampleDarkModeToggle />}
              />
              <Route index path="comboboxes" element={<ExampleComboboxes />} />
              <Route index path="inputtextes" element={<ExampleInputText />} />
              <Route index path="buttons" element={<Buttons />} />
            </Route>
            <Route path="/stocks" element={<Stocks />} /> 
*/
