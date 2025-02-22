import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { useAppDispatch, useAppSelector } from "../../Store/RootState";
import {
  getPortfolioApi,
  ICreatePortfolioRequest,
} from "../../Api/Portfolio.api";
import { Dropdown } from "primereact/dropdown";
import { IPortfolioModel } from "../../Store";
import {
  createPortfolio,
  setShowNewPortfolioForm,
  updatePortfolio,
} from "../../Store/Portfolio.slice";
import { usePortfolio } from "../../Hooks/usePortfolio";
import { useNavigate } from "react-router";

export interface IPortfolioAddEditFormProps {
  portfolioId?: string;
  editMode: boolean;
}

interface IPortfolioAddEditModel {
  name: string;
  description: string;
  bankAccountId: string;
}

export const PortfolioAddEditForm: React.FC<IPortfolioAddEditFormProps> = (
  props
) => {
  const { portfolioId, editMode } = props;

  const dispatch = useAppDispatch();
  const bankAccounts = useAppSelector((x) => x.bankAccounts.list);
  const portfolioItem = usePortfolio(portfolioId);
  const navigate = useNavigate();

  const defaultValues: IPortfolioAddEditModel = {
    name: portfolioItem?.name ?? "",
    description: portfolioItem?.description ?? "",
    bankAccountId: portfolioItem?.bankAccountId ?? "",
  };

  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<IPortfolioAddEditModel>({
    mode: "onChange",
    values: defaultValues,
    defaultValues,
  });

  const onSubmitHandler = handleSubmit((data) => {
    if (!editMode) {
      dispatch(createPortfolio(data));
    } else if (portfolioId) {
      dispatch(updatePortfolio({ ...data, portfolioId }));
    }
  });

  const onCloseHandler = () => {
    navigate(`/portfolios/${portfolioId}`);
  };

  return (
    <form onSubmit={onSubmitHandler}>
      <div className="flex flex-col my-2">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Name
        </label>
        <InputText {...register("name")} className="h-10" />
      </div>

      <div className="flex flex-col my-2">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Description
        </label>
        <InputTextarea {...register("description")} className="h-36" />
      </div>

      <div className="flex flex-col my-2">
        <label className="w-full py-1 text-xs dark:text-white text-black">
          Bank Account
        </label>
        <Controller
          name="bankAccountId"
          control={control}
          render={({ field }) => (
            <Dropdown
              className="h-10"
              {...field}
              optionLabel="name"
              optionValue="id"
              options={bankAccounts}
              placeholder="Select a bound Bank Account"
            />
          )}
        />
      </div>

      <div className="flex my-2">
        <div className="grow"></div>
        <button
          className="grow-0 bg-nav hover:bg-nav/60 text-white px-5 py-2 "
          type="button"
          onClick={onCloseHandler}
        >
          Close
        </button>
        <button
          className="grow-0 bg-green hover:bg-green/60 text-white px-5 py-2 "
          type="submit"
        >
          Save
        </button>
      </div>
    </form>
  );
};
