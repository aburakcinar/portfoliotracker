import React, { useEffect, useState } from "react";
import { SelectButton, SelectButtonChangeEvent } from "primereact/selectbutton";
import { JSX } from "react/jsx-runtime";

import { SunIcon, MoonIcon } from "@heroicons/react/24/outline";

interface IThemeOption {
  name: string;
  value: string;
  icon: JSX.Element;
}

const DarkModeToggle: React.FC = () => {
  const [value, setValue] = useState<string>("dark");

  const options: IThemeOption[] = [
    { name: "Light", value: "light", icon: <SunIcon className="size-4" /> },
    { name: "Dark", value: "dark", icon: <MoonIcon className="size-4" /> },
  ];

  useEffect(() => {
    if (value === "dark") {
      document.body.classList.add("dark");
    } else {
      document.body.classList.remove("dark");
    }
  }, [value]);

  const itemTemplate = (option: IThemeOption) => {
    return option.icon;
  };

  const onChangeHandler = (e: SelectButtonChangeEvent) => {
    setValue(e.value);
  };

  return (
    <SelectButton
      value={value}
      onChange={onChangeHandler}
      options={options}
      itemTemplate={itemTemplate}
    />
  );
};

export { DarkModeToggle };
