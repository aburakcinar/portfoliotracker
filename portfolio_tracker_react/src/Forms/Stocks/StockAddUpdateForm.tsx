import React, { useState } from "react";
import * as Yup from "yup";
import {
  AutoComplete,
  AutoCompleteCompleteEvent,
} from "primereact/autocomplete";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { IExchangeQueryModel, searchExchangesApi } from "../../Api/ExchangeApi";
import { useAppDispatch } from "../../Store/RootState";
import { ILocaleQueryModel, searchLocalesApi } from "../../Api/LocaleApi";
import { Controller, useForm } from "react-hook-form";
import { ICreateAssetStockRequest } from "../../Api/AssetApi";
import { createStockAsset } from "../../Store/AssetsSlice";

export interface IStockAddUpdateFormProps {}

interface IStockAddEditModel {
  stockExchange: IExchangeQueryModel[];
  tickerSymbol: string;
  name: string;
  description: string;
  locale?: ILocaleQueryModel;
  webSite: string;
}

const initialValue: IStockAddEditModel = {
  stockExchange: [],
  tickerSymbol: "",
  name: "",
  description: "",
  locale: undefined,
  webSite: "",
};

export const StockAddUpdateForm: React.FC<IStockAddUpdateFormProps> = (
  props
) => {
  const dispatch = useAppDispatch();

  const [filteredExchanges, setFilteredExchanges] = useState<
    IExchangeQueryModel[]
  >([]);
  const [filteredLocales, setFilteredLocales] = useState<ILocaleQueryModel[]>(
    []
  );

  const searchExchangeHandler = async ($event: AutoCompleteCompleteEvent) => {
    const searchText = $event.query.trim();

    if (searchText.length === 0) {
      setFilteredExchanges([]);
    } else {
      const result = await searchExchangesApi($event.query.trim());

      setFilteredExchanges(
        result.map((x) => {
          return {
            ...x,
            placeholder: `${x.countryCode} - ${x.mic} - ${x.marketNameInstitutionDescription}`,
          };
        })
      );
    }
  };

  const exchangeItemTemplate = (item: IExchangeQueryModel) => {
    return (
      <div className="flex flex-col">
        <div className="text-sm">{item.marketNameInstitutionDescription}</div>
        <div className="text-xs">
          {item.mic} - <span className="italic">{item.city}</span>
        </div>
      </div>
    );
  };

  const searchLocaleHandler = async ($event: AutoCompleteCompleteEvent) => {
    const searchText = $event.query.trim();

    if (searchText.length === 0) {
      setFilteredLocales([]);
    } else {
      const result = await searchLocalesApi($event.query.trim(), 10);

      setFilteredLocales(
        result.map((x) => {
          return {
            ...x,
            placeholder: `${x.countryCode} - ${x.currencyCode} - ${x.countryName}`,
          };
        })
      );
    }
  };

  const localeItemTemplate = (item: ILocaleQueryModel) => {
    return (
      <div className="flex flex-col">
        <div className="text-sm">
          {item.countryCode} - {item.countryName}{" "}
        </div>
        <div className="text-xs">
          {item.currencySymbol} -{" "}
          <span className="italic">{item.currencyCode}</span>
        </div>
      </div>
    );
  };

  const {
    register,
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<IStockAddEditModel>({
    defaultValues: initialValue,
  });
  const onSubmitHandler = handleSubmit((data) => {
    if (data.stockExchange.length !== 1 || !data.locale) {
      return;
    }

    const { stockExchange, tickerSymbol, name, description, locale, webSite } =
      data;

    const { currencyCode } = locale;

    const createRequest: ICreateAssetStockRequest = {
      exchangeCode: stockExchange[0].mic,
      tickerSymbol,
      currencyCode,
      name,
      description,
      webSite,
    };

    console.log(createRequest);
    dispatch(createStockAsset(createRequest));
  });

  return (
    <form onSubmit={onSubmitHandler}>
      <div className="flex w-full flex-col">
        <div className="text-green text-2xl mb-5">New Stock</div>
        <div className="flex flex-col ">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Stock Exchange
          </label>
          <Controller
            name="stockExchange"
            control={control}
            render={({ field }) => (
              <AutoComplete
                className="w-full h-12"
                field="placeholder"
                {...field}
                suggestions={filteredExchanges}
                completeMethod={searchExchangeHandler}
                itemTemplate={exchangeItemTemplate}
                delay={500}
                multiple
                selectionLimit={1}
              />
            )}
          />
        </div>

        <div className="flex flex-col ">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Locale
          </label>
          <Controller
            name="locale"
            control={control}
            render={({ field }) => (
              <AutoComplete
                className="w-full h-12"
                field="placeholder"
                {...field}
                suggestions={filteredLocales}
                completeMethod={searchLocaleHandler}
                itemTemplate={localeItemTemplate}
                delay={500}
              />
            )}
          />
        </div>

        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Stock Symbol
          </label>
          <InputText {...register("tickerSymbol")} className="h-12" />
        </div>

        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Stock Name
          </label>
          <InputText {...register("name")} className="h-12" />
        </div>

        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Website
          </label>
          <InputText {...register("webSite")} className="h-12" />
        </div>

        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Description
          </label>
          <InputTextarea {...register("description")} className="h-36" />
        </div>

        <div className="flex my-2">
          <div className="grow"></div>
          <button
            className="grow-0 bg-green hover:bg-green/60 text-white px-5 py-2 "
            type="submit"
          >
            Save
          </button>
        </div>
      </div>
    </form>
  );
};
