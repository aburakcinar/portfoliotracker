import { Card } from "primereact/card";
import React from "react";
import { useForm } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { setShowNewPortfolioForm, useAppDispatch } from "../../Store/RootState";
import { createPortfolio } from "../../Store/Thunks";
import { ICreatePortfolioRequest } from "../../Api/PortfolioV2Api";

export const NewPortfolio: React.FC = () => {
  const dispatch = useAppDispatch();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ICreatePortfolioRequest>({
    defaultValues: { name: "", description: "" },
  });

  const onSubmitHandler = handleSubmit((data) => {
    dispatch(createPortfolio(data));
  });

  const onCloseHandler = () => {
    dispatch(setShowNewPortfolioForm(false));
  };

  return (
    <Card title="New Portfolio">
      <form onSubmit={onSubmitHandler}>
        <div className="flex flex-col my-2">
          <label className="w-full py-1 text-xs dark:text-white text-black">
            Name
          </label>
          <InputText {...register("name")} className="h-12" />
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
    </Card>
  );
};
