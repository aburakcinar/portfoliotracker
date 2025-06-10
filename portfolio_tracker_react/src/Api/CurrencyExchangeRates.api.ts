import api, { toQueryString } from "./Api";

export interface IGetCurrencyRatesTimeseriesQuery {
  startDate: Date;
  endDate: Date;
  baseCurrency: string;
  targetCurrency: string;
}

export interface ICurrencyRateItem {
  date: Date;
  rate: number;
}

export interface IGetCurrencyRatesTimeseriesResult {
  success: boolean;
  requestName: string;
  baseCurrency: string;
  targetCurrency: string;
  rates: ICurrencyRateItem[];
}

export const queryCurrencyExchangeRates = async (
  query: IGetCurrencyRatesTimeseriesQuery
): Promise<IGetCurrencyRatesTimeseriesResult> => {
  const { startDate, endDate } = query;
  const queryCorrected = {
    ...query,
    startDate: `${(startDate.getMonth() + 1)
      .toString()
      .padStart(2, "0")}-${startDate
      .getDate()
      .toString()
      .padStart(2, "0")}-${startDate.getFullYear()}`,
    endDate: `${(endDate.getMonth() + 1).toString().padStart(2, "0")}-${endDate
      .getDate()
      .toString()
      .padStart(2, "0")}-${endDate.getFullYear()}`,
  };
  const url = `/timeseries?${toQueryString(queryCorrected)}`;

  console.log(url);

  const response = await api.get<IGetCurrencyRatesTimeseriesResult>(url);

  return response.data;
};
