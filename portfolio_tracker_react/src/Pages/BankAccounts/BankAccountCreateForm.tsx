import { Card } from "primereact/card";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { ICreateBankAccountCommand } from "../../Api/BankAccount.api";
import { InputText } from "primereact/inputtext";
import { InputMask } from "primereact/inputmask";
import { InputTextarea } from "primereact/inputtextarea";
import { getCurrencies, ICurrencyItem } from "../../Api";
import {
  AutoComplete,
  AutoCompleteCompleteEvent,
} from "primereact/autocomplete";
import { Calendar } from "primereact/calendar";
import { useAppDispatch } from "../../Store/RootState";
import { createBankAccount } from "../../Store/BankAccount.slice";

export const BankAccountCreateForm: React.FC = () => {
  const dispatch = useAppDispatch();
  const [currencies, setCurrencies] = useState<ICurrencyItem[]>([]);
  const [filteredCurrencies, setFilteredCurrencies] = useState<ICurrencyItem[]>(
    []
  );

  useEffect(() => {
    const fetch = async () => {
      const results = await getCurrencies();
      setCurrencies(results);
    };

    fetch();
  }, []);

  const initialValue = {
    name: "",
    bankName: "",
    accountHolder: "",
    description: "",
    iban: "",
    currencyCode: "EUR",
    localeCode: "",
    openDate: new Date(),
  } satisfies ICreateBankAccountCommand;

  const {
    register,
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<ICreateBankAccountCommand>({
    defaultValues: initialValue,
  });

  const onSubmitHandler = handleSubmit((data) => {
    console.log(data);

    dispatch(createBankAccount(data));
  });

  const searchCurrencyHandler = async ($event: AutoCompleteCompleteEvent) => {
    const searchText = $event.query.toLowerCase().trim();

    if (searchText.length === 0) {
      setFilteredCurrencies([]);
    } else {
      const result = currencies.filter(
        (x) =>
          x.code.toLowerCase().includes(searchText) ||
          x.name.toLowerCase().includes(searchText) ||
          x.nameLocal.toLowerCase().includes(searchText)
      );

      setFilteredCurrencies(
        result.map((x) => {
          return {
            ...x,
            placeholder: `${x.code} - ${x.name}`,
          };
        })
      );
    }
  };

  const currencyItemTemplate = (currencyCode: string) => {
    const item = currencies.find((x) => x.code === currencyCode);

    if (item) {
      return (
        <div className="flex text-sm">
          {item.code} - {item.name}
        </div>
      );
    }
  };

  return (
    <div className="flex flex-col min-w-[500px] w-1/2 ">
      <h2 className="text-green pb-8 text-5xl">New Bank Account</h2>
      <Card>
        <form onSubmit={onSubmitHandler}>
          <div className="flex w-full flex-col">
            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Account Name
              </label>
              <InputText {...register("name")} className="h-10" />
            </div>

            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Bank Name
              </label>
              <InputText {...register("bankName")} className="h-10" />
            </div>

            <div className="flex flex-col my-2">
              <label className="w-full py-1 text-xs dark:text-white text-black">
                Account Holder
              </label>
              <InputText {...register("accountHolder")} className="h-10" />
            </div>

            <div className="grid grid-cols-2 gap-2">
              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  IBAN
                </label>
                <Controller
                  name="iban"
                  control={control}
                  render={({ field }) => (
                    <InputMask
                      {...field}
                      mask="aa99 9999 9999 9999 9999 99"
                      placeholder="Enter IBAN"
                      slotChar="_"
                      className="h-10"
                    />
                  )}
                />
              </div>
              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  Open Date
                </label>
                <Calendar
                  {...register("openDate")}
                  dateFormat="dd/mm/yy"
                  className="h-10"
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-2">
              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  Currency
                </label>
                {/* <InputText {...register("currencyCode")} className="h-12" /> */}
                <Controller
                  name="currencyCode"
                  control={control}
                  render={({ field }) => (
                    <AutoComplete
                      className="w-full h-10"
                      {...field}
                      suggestions={filteredCurrencies.map((x) => x.code)}
                      completeMethod={searchCurrencyHandler}
                      itemTemplate={currencyItemTemplate}
                      delay={100}
                    />
                  )}
                />
              </div>

              <div className="flex flex-col my-2">
                <label className="w-full py-1 text-xs dark:text-white text-black">
                  Country
                </label>
                <InputText {...register("localeCode")} className="h-10" />
              </div>
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
      </Card>
    </div>
  );
};
