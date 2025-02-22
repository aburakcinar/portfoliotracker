import { createRoot } from "react-dom/client";
import Home from "./Home";
import { BrowserRouter, Route, Routes } from "react-router";

import { PrimeReactProvider } from "primereact/api";

import "./tailwind.css";
import { Layout } from "./Layout";
import { appDesignSystem } from "./Styles/AppDesignSystem";
import { Provider } from "react-redux";
import { store } from "./Store/RootState";
import { AssetViewForm } from "./Pages/Assets/AssetViewForm/AssetViewForm";
import {
  PortfolioList,
  PortfolioLayout,
  PortfolioItem,
} from "./Pages/Portfolio";
import { BankAccountList } from "./Pages/BankAccounts/BankAccountList";
import { BankAccountLayout } from "./Pages/BankAccounts/BankAccountLayout";
import { BankAccountCreateForm } from "./Pages/BankAccounts/BankAccountCreateForm";
import { BankAccountDetail } from "./Pages/BankAccounts/BankAccountDetail";
import { EditPortfolio } from "./Pages/Portfolio/EditPortfolio";
import { AddHoldingPage } from "./Pages/Holding/AddHoldingForm";
import {
  AssetsLayout,
  NewAssetPage,
  AssetListPage,
  AssetsSummary,
} from "./Pages/Assets/";
import { HoldingDetailPage } from "./Pages/Holding/HoldingDetailForm";

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
              <Route
                path=":portfolioId/addholding"
                element={<AddHoldingPage />}
              />
              <Route path=":portfolioId/edit" element={<EditPortfolio />} />
              <Route
                path=":portfolioId/holding/:assetId"
                element={<HoldingDetailPage />}
              />
            </Route>

            <Route path="assets" element={<AssetsLayout />}>
              <Route index element={<AssetsSummary />} />
              <Route path=":assetTypeName" element={<AssetListPage />} />
              <Route path=":assetTypeName/new" element={<NewAssetPage />} />
              <Route path=":assetTypeName/:id" element={<AssetViewForm />} />
              {/* <Route path="stocks" element={<AssetsStocks />} />
              <Route path="stocks/newstock" element={<NewStock />} />
              <Route path="stocks/:id" element={<AssetViewForm />} />
              <Route path="etfs" element={<AssetsEtfs />} />
              <Route path="commodities" element={<AssetsCommodities />} /> */}
            </Route>

            <Route path="bankaccounts" element={<BankAccountLayout />}>
              <Route index element={<BankAccountList />} />
              <Route path="new" element={<BankAccountCreateForm />} />
              <Route path="detail/:id" element={<BankAccountDetail />} />
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
