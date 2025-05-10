import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";
import { Provider } from "react-redux";
import { PrimeReactProvider } from "primereact/api";

import "./tailwind.css";
import Home from "./Home";
import { Layout } from "./Layout";
import { appDesignSystem } from "./Styles/AppDesignSystem";
import { store } from "./Store";
import { AssetViewForm } from "./Pages/Assets/AssetViewForm/AssetViewForm";
import {
  PortfolioList,
  PortfolioLayout,
  PortfolioItem,
} from "./Pages/Portfolio";
import {
  BankAccountList,
  BankAccountLayout,
  BankAccountCreateForm,
  BankAccountDetail,
  ImportBankAccountsForm,
} from "./Pages/BankAccounts";
import { EditPortfolio } from "./Pages/Portfolio";
import {
  AssetsLayout,
  NewAssetPage,
  AssetListPage,
  AssetsSummary,
} from "./Pages/Assets/";
import { HoldingDetailPage, AddHoldingPage } from "./Pages/Holding";
import { ExchangeRatesPage } from "./Pages/Currencies/ExchangeRatesPage";

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
            </Route>

            <Route path="bankaccounts" element={<BankAccountLayout />}>
              <Route index element={<BankAccountList />} />
              <Route path="new" element={<BankAccountCreateForm />} />
              <Route path=":id" element={<BankAccountDetail />} />
              <Route path="import" element={<ImportBankAccountsForm />} />
            </Route>

            <Route path="exchangerates" element={<ExchangeRatesPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </PrimeReactProvider>
  </Provider>
);
