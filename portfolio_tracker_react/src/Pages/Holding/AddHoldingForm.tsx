import React from "react";
import { useNavigate, useParams } from "react-router";
import { Card } from "primereact/card";
import { Controller, useForm } from "react-hook-form";
import { AssetPicker } from "../../Controls/AssetPicker";
import { InputNumber } from "primereact/inputnumber";
import { MinusCircleIcon, PlusCircleIcon } from "@heroicons/react/24/outline";
import { Calendar } from "primereact/calendar";
import { usePortfolio } from "../../Hooks/usePortfolio";
import { addHoldingApi, IAddHoldingCommand } from "../../Api/HoldingsApi";
import { PortfolioInfoForm } from "../Portfolio/PortfolioInfoForm";
import { Button } from "primereact/button";

export interface IAddHoldingFormProps {
  portfolioId: string;
  assetId?: string;
  onChange?: () => void;
  className?: string;
  title?: string;
}

export const AddHoldingForm: React.FC<IAddHoldingFormProps> = (props) => {
  const { portfolioId, assetId, onChange } = props;

  const portfolio = usePortfolio(portfolioId);

  const defaultValues = {
    assetId: assetId ?? "",
    executeDate: new Date().toISOString(),
    executeDateTime: new Date(),
    quantity: 0,
    price: 0,
    expenses: 0,
    portfolioId: portfolioId ?? "",
  } satisfies IAddHoldingCommand;

  const {
    handleSubmit,
    control,
    formState: { errors },
    setValue,
    watch,
  } = useForm<IAddHoldingCommand>({
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
    } satisfies IAddHoldingCommand;

    console.log(finalData);
    addHoldingApi(finalData)
      .then((result) => {
        if (onChange) {
          onChange();
        }
      })
      .catch(console.log);
  });

  return (
    <Card title={props.title ?? "New Holding"} className={props.className}>
      <form onSubmit={onSubmitHandler}>
        {!assetId && (
          <div className="flex flex-col my-2">
            <label className="w-full py-1 text-xs dark:text-white text-black">
              Asset
            </label>
            <Controller
              name="assetId"
              control={control}
              render={({ field }) => <AssetPicker {...field} />}
            />
          </div>
        )}

        {(!!assetId || !!ongoingValues.assetId) && (
          <div className="grid grid-cols-2 gap-1 my-2">
            <div className="flex flex-col ">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Buy Date
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
                Quantity
              </label>
              <Controller
                name="quantity"
                control={control}
                render={({ field }) => (
                  <InputNumber
                    {...field}
                    onChange={(e) => setValue("quantity", e.value ?? 0)}
                    className="h-10"
                    showButtons
                    buttonLayout="horizontal"
                    min={0}
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
          </div>
        )}

        <div className="flex my-2">
          <div className="grow"></div>

          <Button
            className="grow-0 bg-green hover:bg-green/60  px-5 py-2 "
            type="submit"
          >
            Save
          </Button>
        </div>
      </form>
    </Card>
  );
};

export const AddHoldingPage: React.FC = () => {
  const { portfolioId } = useParams();
  const navigate = useNavigate();

  if (!portfolioId) {
    return null;
  }

  const onAddHoldingHandler = () => {
    navigate(`/portfolios/${portfolioId}`);
  };

  return (
    <div className="flex min-w-[500px] w-1/2  mt-5 flex-col ">
      <span className="text-green text-4xl py-4">Add Holding</span>
      <PortfolioInfoForm portfolioId={portfolioId} />
      <AddHoldingForm
        portfolioId={portfolioId}
        onChange={onAddHoldingHandler}
      />
    </div>
  );
};
