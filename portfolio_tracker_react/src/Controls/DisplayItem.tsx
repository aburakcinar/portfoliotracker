import React, { useEffect, useState } from "react";
import { InputText } from "primereact/inputtext";
import { classNames } from "primereact/utils";
import { PencilIcon, CheckIcon, XMarkIcon } from "@heroicons/react/24/outline";

export interface IDisplayItemProps {
  label: string;
  content: string | null | undefined;
  className?: string;
  labelClassName?: string;
  contentClassName?: string;
  editable?: boolean;
  onEdit?: (value: string) => void;
}

export const DisplayItem: React.FC<IDisplayItemProps> = (props) => {
  const {
    label,
    content,
    className,
    contentClassName,
    labelClassName,
    editable,
    onEdit,
  } = props;

  const [value, setValue] = useState<string>(content ?? "");
  const [editMode, setEditMode] = useState<boolean>(false);

  useEffect(() => {
    setValue(content ?? "");
  }, [content]);

  const onEditHandler = () => {
    if (onEdit) {
      onEdit(value);
    }
    setEditMode(false);
  };

  return (
    <div className={classNames("flex flex-col", className)}>
      <label
        className={classNames(
          "w-full py-1 text-xs dark:text-white text-black",
          labelClassName
        )}
      >
        {label}
      </label>
      <div className="relative">
        <InputText
          className={classNames(contentClassName, "w-full")}
          readOnly={!editMode}
          value={value}
          onChange={(e) => setValue(e.currentTarget.value)}
        />
        {(editable ?? false) && (
          <span className="absolute top-1.5 right-1">
            {!editMode && (
              <button
                className="opacity-0 hover:opacity-100"
                onClick={(_) => setEditMode(true)}
              >
                <PencilIcon className="size-5" />
              </button>
            )}
            {editMode && (
              <>
                <button className="" onClick={onEditHandler}>
                  <CheckIcon className="size-6 text-green dark:text-green hover:text-green/60 dark:hover:text-green/60" />
                </button>
                <button className="" onClick={(_) => setEditMode(false)}>
                  <XMarkIcon className="size-6 text-red-700 dark:text-red-700  hover:text-red-500 dark:hover:text-red-500" />
                </button>
              </>
            )}
          </span>
        )}
      </div>
    </div>
  );
};
