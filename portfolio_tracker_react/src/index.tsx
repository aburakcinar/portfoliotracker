import React from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { BrowserRouter, Route, Routes } from "react-router";

import "./tailwind.css";
import { Layout } from "./Layout";
import Transactions from "./Pages/Transactions";
import Portfolio from "./Pages/Portfolio";

const rootElem = document.getElementById("root");
const root = createRoot(rootElem!);
root.render(
  <BrowserRouter>
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<App />} />
        <Route path="/Portfolio" element={<Portfolio />} />
        <Route path="/transactions" element={<Transactions />} />
      </Route>
    </Routes>
  </BrowserRouter>
);
