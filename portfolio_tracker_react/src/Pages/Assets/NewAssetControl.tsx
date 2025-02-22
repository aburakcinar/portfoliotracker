import React from "react";
import { InputText } from "primereact/inputtext";
import { getExchangeApi } from "../../Api/Exchange.api";
import { Controller, useForm } from "react-hook-form";
import {
  createAssetApi,
  IAssetSummaryModel,
  ICreateAssetCommand,
} from "../../Api/Asset.api";
import { useNavigate } from "react-router";
import { ExchangePicker } from "../../Controls/ExchangePicker";

export interface INewAssetControlProps {
  assetType: IAssetSummaryModel;
}

export const NewAssetControl: React.FC<INewAssetControlProps> = (props) => {
  const navigate = useNavigate();

  const { assetType } = props;

  const defaultValues = {
    assetType: assetType.assetTypeId,
    tickerSymbol: "",
    name: "",
    exchangeCode: "",
    currencyCode: "",
  } satisfies ICreateAssetCommand;

  const {
    register,
    control,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<ICreateAssetCommand>({
    defaultValues,
  });

  const onSubmitHandler = handleSubmit((data) => {
    createAssetApi(data)
      .then((val) => {
        navigate(`/assets/${assetType.assetType.toLowerCase()}`);
      })
      .catch(console.log);
  });

  const onExchangeChangeHandler = (val: any) => {
    setValue("exchangeCode", val);

    getExchangeApi(val)
      .then((item) => {
        if (item) {
          setValue("currencyCode", item.currencyCode);
        }
      })
      .catch(console.log);
  };

  return (
    <form onSubmit={onSubmitHandler}>
      <div className="flex w-full flex-col">
        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Ticker
          </label>
          <InputText {...register("tickerSymbol")} className="h-10" />
        </div>

        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Name
          </label>
          <InputText {...register("name")} className="h-10" />
        </div>

        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black my-2">
            Stock Exchange
          </label>
          <Controller
            name="exchangeCode"
            control={control}
            render={({ field }) => (
              <ExchangePicker {...field} onChange={onExchangeChangeHandler} />
            )}
          />
        </div>

        <div className="flex flex-col ">
          <label className="w-full py-1 text-xs dark:text-white text-black my-2">
            Currency
          </label>
          <Controller
            name="currencyCode"
            control={control}
            render={({ field }) => (
              <InputText {...field} readOnly className="h-10" />
            )}
          />
        </div>

        <div className="flex my-2 mt-10">
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
