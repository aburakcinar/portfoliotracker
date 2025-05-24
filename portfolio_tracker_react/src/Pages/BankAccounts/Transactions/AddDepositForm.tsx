import { MinusCircleIcon } from "@heroicons/react/24/outline";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Card } from "primereact/card";
import { InputNumber } from "primereact/inputnumber";
import React, { useState } from "react";
import { Controller, useForm } from "react-hook-form";

export interface IAddDepositFormProps {
    bankAccountId: string;
    currencyCode: string;
}

interface IAddDepositFormValues {
    executeDate: Date;
    amount: number;
    tax: number;
    fee: number;
}

export const AddDepositForm: React.FC<IAddDepositFormProps> = (props) => {

    const { bankAccountId, currencyCode } = props;
    const [showTax, setShowTax] = useState<boolean>(true);
    const [showFee, setShowFee] = useState<boolean>(true);


    const {
        register,
        control,
        handleSubmit,
        formState: { errors },
        setValue,
    } = useForm<IAddDepositFormValues>({
        defaultValues: {
            amount: 0,
            tax: 0,
            fee: 0
        },
    });

    const onSubmitHandler = handleSubmit((data) => {
        console.log(data);

        // dispatch(createBankAccount(data));
    });

    return (
        <Card title="Add Deposit/Withdraw">
            <form onSubmit={onSubmitHandler}>
                <div className="flex flex-col gap-2">
                    <div >
                        <label className="w-full py-1 text-xs dark:text-white text-black">
                            Execute Date
                        </label>
                        <Calendar
                            {...register("executeDate")}
                            dateFormat="dd/mm/yy"
                            className="h-10"
                        />
                    </div>
                    <div>
                        <label htmlFor="amount">Amount</label>
                        <Controller
                            name="amount"
                            control={control}
                            render={({ field }) => (
                                <InputNumber
                                    {...field}
                                    className="h-10"
                                    onChange={(e) => setValue("amount", e.value ?? 0)}
                                    mode="currency"
                                    currency={currencyCode}
                                />
                            )}
                        />
                    </div>
                    {showTax && <div>
                        <label htmlFor="tax">Tax</label>
                        <div className="flex flex-row">
                            <Controller
                                name="tax"
                                control={control}
                                render={({ field }) => (
                                    <InputNumber
                                        {...field}
                                        className="h-10 grow"
                                        onChange={(e) => setValue("tax", e.value ?? 0)}
                                        mode="currency"
                                        currency={currencyCode}
                                    />
                                )}
                            />
                            <Button
                                icon={<MinusCircleIcon className="size-6 text-red-500" title="Remove" />}
                                className="grow-0 bg-transparent!  p-0! px-2!  text-red-500"
                                onClick={_ => setShowTax(false)}
                            />
                        </div>
                    </div>}
                    {showFee && <div>
                        <label htmlFor="fee">Fee</label>
                        <div className="flex flex-row">
                            <Controller
                                name="fee"
                                control={control}
                                render={({ field }) => (
                                    <InputNumber
                                        {...field}
                                        className="h-10 grow"
                                        onChange={(e) => setValue("fee", e.value ?? 0)}
                                        mode="currency"
                                        currency={currencyCode}
                                    />
                                )}
                            />
                            <Button
                                icon={<MinusCircleIcon className="size-6 text-red-500" title="Remove" />}
                                className="grow-0 bg-transparent!  p-0! px-2!  text-red-500"
                                onClick={_ => setShowFee(false)}
                            />
                        </div>
                    </div>}
                    <div className="flex gap-2">
                        {!showTax && <Button label="Add Tax" className="h-7 text-xs" onClick={_ => setShowTax(true)} />}
                        {!showFee && <Button label="Add Fee" className="h-7 text-xs" onClick={_ => setShowFee(true)} />}
                    </div>
                    <div className="flex flex-row-reverse">
                        <Button label="Save" type="submit" />
                    </div>
                </div>
            </form>
        </Card>
    );
};

