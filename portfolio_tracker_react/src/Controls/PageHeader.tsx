import { classNames } from "primereact/utils";
import React from "react";

export interface IPageHeaderProps {
  title: string;
  className?: string;
  capitalize?: boolean;
}

export const PageHeader: React.FC<IPageHeaderProps> = (props) => {
  const { title, className, capitalize } = props;

  const divClassName = classNames(
    "text-green text-5xl py-2 ",
    { capitalize: capitalize ?? false },
    className
  );

  return <div className={divClassName}>{title}</div>;
};
