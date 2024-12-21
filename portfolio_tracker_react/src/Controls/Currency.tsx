import classNames from "classnames";
import React from "react";

interface ICurrencyProps {
  value: number;
  className?: string;
}

export function Currency(props: Readonly<ICurrencyProps>) {
  const { value, className } = props;

  const integerPart = Math.floor(value);
  const fractionalPart = Number((value - integerPart).toFixed(2)) * 100;

  const mainClassName = classNames(
    { className: !!className },
    "text-[17px]",
    "grid content-start grid-cols-2 align-top"
  );

  return (
    <div className={mainClassName}>
      <div className="text-right relative">{integerPart}</div>
      <div className="text-[12px] relative text-left">
        {fractionalPart.toFixed(2).toString()}
      </div>
    </div>
  );
}
