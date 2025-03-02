import React from "react";
import { usePortfolio } from "../../Hooks";
import { Card } from "primereact/card";
import { Controller, useForm } from "react-hook-form";
import { InputNumber } from "primereact/inputnumber";
import { MinusCircleIcon, PlusCircleIcon } from "@heroicons/react/24/outline";
import { Calendar } from "primereact/calendar";
import { useHoldingDetail } from "../../Hooks/useHoldingDetail";
import { Button } from "primereact/button";
import { ISellHoldingCommand, sellHoldingApi } from "../../Api/HoldingsApi";

export interface IReportSellHoldingFormProps {
  portfolioId: string;
  assetId?: string;
  onChange?: () => void;
  className?: string;
  title?: string;
}

export const ReportSellHoldingForm: React.FC<IReportSellHoldingFormProps> = (
  props
) => {
  const { portfolioId, assetId, onChange } = props;

  const portfolio = usePortfolio(portfolioId);
  const holdingDetails = useHoldingDetail(portfolioId, assetId, []);

  const totalQuantity = holdingDetails.reduce(
    (sum, detail) => sum + detail.quantity,
    0
  );

  const defaultValues = {
    assetId: assetId ?? "",
    executeDate: new Date().toISOString(),
    executeDateTime: new Date(),
    quantity: 0,
    price: 0,
    expenses: 0,
    taxes: 0,
    portfolioId: portfolioId ?? "",
  } satisfies ISellHoldingCommand;

  const {
    handleSubmit,
    control,
    formState: { errors },
    setValue,
    watch,
  } = useForm<ISellHoldingCommand>({
    mode: "all",
    defaultValues,
  });

  if (!portfolio) {
    return null;
  }

  const ongoingValues = watch();

  const onSubmitHandler = handleSubmit((data) => {
    const finalData = {
      ...data,
      assetId: data.assetId ?? assetId ?? "",
      executeDate: data.executeDateTime.toISOString(),
    } satisfies ISellHoldingCommand;

    console.log(finalData);
    sellHoldingApi(finalData)
      .then((result) => {
        if (result && onChange) {
          onChange();
        }
      })
      .catch(console.log);
  });

  return (
    <Card title={props.title ?? "Sell Holding"} className={props.className}>
      <form onSubmit={onSubmitHandler}>
        <div className="grid grid-cols-2 gap-1 my-2">
          <div className="flex flex-col ">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Sell Date
            </label>
            <Controller
              name="executeDateTime"
              control={control}
              render={({ field }) => (
                <Calendar {...field} dateFormat="dd/mm/yy" className="h-10" />
              )}
            />
          </div>
          <div className="flex flex-col ">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Quantity (Max: {totalQuantity})
            </label>
            <Controller
              name="quantity"
              control={control}
              rules={{ max: totalQuantity }}
              render={({ field }) => (
                <InputNumber
                  {...field}
                  onChange={(e) => setValue("quantity", e.value ?? 0)}
                  className="h-10"
                  showButtons
                  buttonLayout="horizontal"
                  min={0}
                  max={totalQuantity}
                  step={1}
                  incrementButtonIcon={() => (
                    <PlusCircleIcon className="size-7" />
                  )}
                  decrementButtonIcon={() => (
                    <MinusCircleIcon className="size-7" />
                  )}
                  mode="decimal"
                />
              )}
            />
          </div>
          <div className="flex flex-col">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Price
            </label>
            <Controller
              name="price"
              control={control}
              render={({ field }) => (
                <InputNumber
                  {...field}
                  className="h-10"
                  onChange={(e) => setValue("price", e.value ?? 0)}
                  mode="currency"
                  currency={portfolio!.currencyCode}
                />
              )}
            />
          </div>
          <div className="flex flex-col">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Expenses
            </label>
            <Controller
              name="expenses"
              control={control}
              render={({ field }) => (
                <InputNumber
                  {...field}
                  className="h-10"
                  onChange={(e) => setValue("expenses", e.value ?? 0)}
                  mode="currency"
                  currency={portfolio!.currencyCode}
                />
              )}
            />
          </div>
          <div className="flex flex-col">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Taxes
            </label>
            <Controller
              name="taxes"
              control={control}
              render={({ field }) => (
                <InputNumber
                  {...field}
                  className="h-10"
                  onChange={(e) => setValue("taxes", e.value ?? 0)}
                  mode="currency"
                  currency={portfolio!.currencyCode}
                />
              )}
            />
          </div>
        </div>

        <div className="flex my-2">
          <div className="grow"></div>
          <Button
            className="grow-0 bg-green hover:bg-green/60 px-5 py-2"
            type="submit"
          >
            Sell
          </Button>
        </div>
      </form>
    </Card>
  );
};
