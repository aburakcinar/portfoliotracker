import React, { useEffect, useState } from "react";
import { IAssetModel, updateAssetFieldApi } from "../../../Api/Asset.api";
import { PencilIcon, CheckIcon, XMarkIcon } from "@heroicons/react/24/outline";
import { Card } from "primereact/card";
import { InputTextarea } from "primereact/inputtextarea";
import classNames from "classnames";

export interface IAssetViewDescriptionControlProps {
  asset: IAssetModel | null;
  onUpdate?: () => void;
}

export const AssetViewDescriptionControl: React.FC<
  IAssetViewDescriptionControlProps
> = (props) => {
  const { asset, onUpdate } = props;
  const [editing, setEditing] = useState<boolean>(false);
  const [description, setDescription] = useState<string>(
    asset?.description ?? ""
  );
  const [textLength, setTextLength] = useState<number>(description.length);
  const [error, setError] = useState<boolean>(false);

  useEffect(() => {
    setTextLength(description.length);
    setError(false);
  }, [description]);

  if (!asset) {
    return null;
  }

  const onUpdateHandler = () => {
    const update = async () => {
      var result = await updateAssetFieldApi(
        asset.id,
        "Description",
        description
      );

      if (result) {
        setEditing(false);
        if (onUpdate) {
          onUpdate();
        }
      } else {
        setError(true);
      }
    };

    update();
  };

  const onDescriptionChangeHandler = (text: string) => {
    setDescription(text.length > 2000 ? text.substring(0, 2000) : text);
  };

  return (
    <Card
      title={
        <div className="flex flex-row w-full">
          <label className="grow ">About</label>
          {!editing && (
            <button className="grow-0" onClick={(_) => setEditing(true)}>
              <PencilIcon className="size-5 hover:text-green dark:hover:text-green" />
            </button>
          )}
          {editing && (
            <>
              <button className="grow-0" onClick={(_) => onUpdateHandler()}>
                <CheckIcon className="size-6 text-green dark:text-green hover:text-green/60 dark:hover:text-green/60" />
              </button>
              <button className="grow-0" onClick={(_) => setEditing(false)}>
                <XMarkIcon className="size-6 text-red-700 dark:text-red-700  hover:text-red-500 dark:hover:text-red-500" />
              </button>
            </>
          )}
        </div>
      }
      className="col-span-8 text-black dark:text-white/60 flex flex-col"
    >
      {!editing && <div className="mt-2">{description}</div>}
      {editing && (
        <div className="flex flex-col">
          <InputTextarea
            className={classNames("w-full h-72", {
              "border-red-800 dark:border-red-800": error,
            })}
            value={description}
            onChange={(e) => onDescriptionChangeHandler(e.currentTarget.value)}
          />
          <span className="w-full text-right">{textLength}/2000</span>
        </div>
      )}
    </Card>
  );
};
