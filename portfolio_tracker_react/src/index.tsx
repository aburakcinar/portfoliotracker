import React from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { BrowserRouter, Route, Routes } from "react-router";

import { PrimeReactProvider } from "primereact/api";

import "./tailwind.css";
import { Layout } from "./Layout";
import Transactions from "./Pages/Transactions";
import Portfolio from "./Pages/Portfolio";
import { appDesignSystem } from "./Styles/AppDesignSystem";
import { ExampleControls } from "./Pages/ExampleControls";
import { Provider } from "react-redux";
import { store } from "./Store/RootState";
import Stocks from "./Pages/Stocks";

const rootElem = document.getElementById("root");
const root = createRoot(rootElem!);
root.render(
  <Provider store={store}>
    <PrimeReactProvider value={{ unstyled: true, pt: appDesignSystem }}>
      <BrowserRouter>
        <Routes>
          <Route element={<Layout />}>
            <Route path="/" element={<App />} />
            <Route path="/portfolio" element={<Portfolio />} />
            <Route path="/transactions" element={<Transactions />} />
            <Route path="/controls" element={<ExampleControls />} />
            <Route path="/stocks" element={<Stocks />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </PrimeReactProvider>
  </Provider>
);
