import { useMemo } from "react";

const currencySymbols: { [key: string]: string } = {
  USD: "$",
  EUR: "€",
  GBP: "£",
  JPY: "¥",
  CNY: "¥",
  KRW: "₩",
  INR: "₹",
  RUB: "₽",
  BRL: "R$",
  CHF: "Fr",
  AUD: "A$",
  CAD: "C$",
  HKD: "HK$",
  NZD: "NZ$",
  SGD: "S$",
  TRY: "₺",
  // Add more currency symbols as needed
};

export const useCurrencySymbol = (currencyCode: string): string => {
  return useMemo(() => {
    const upperCaseCode = currencyCode.toUpperCase();
    return currencySymbols[upperCaseCode] || upperCaseCode;
  }, [currencyCode]);
};

export default useCurrencySymbol;
