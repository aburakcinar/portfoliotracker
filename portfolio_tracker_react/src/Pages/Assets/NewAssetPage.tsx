import React from "react";
import { PageHeader } from "../../Controls/PageHeader";
import { Card } from "primereact/card";
import { useAssetTypeByName, useMenuItem } from "../../Hooks";
import { useParams } from "react-router";
import { IAssetSummaryModel } from "../../Api/Asset.api";
import { NewAssetControl } from "./NewAssetControl";

export const NewAssetPage: React.FC = () => {
  const { assetTypeName } = useParams();
  const assetType = useAssetTypeByName(assetTypeName);

  if (!assetType) {
    return null;
  }

  return <NewAssetForm assetType={assetType} />;
};

export interface INewAssetFormProps {
  assetType: IAssetSummaryModel;
}

export const NewAssetForm: React.FC<INewAssetFormProps> = (props) => {
  const { assetType } = props;

  useMenuItem(
    [
      {
        id: `/assets/${assetType.assetType.toLowerCase()}`,
        link: `/assets/${assetType.assetType.toLowerCase()}`,
        text: assetType.assetType,
      },
      {
        id: `/assets/${assetType.assetType.toLowerCase()}/new`,
        link: `/assets/${assetType.assetType.toLowerCase()}/new`,
        text: "New",
      },
    ],
    [assetType.title]
  );

  return (
    <div className="flex min-w-[500px] w-1/2  mt-5 flex-col ">
      <PageHeader title={`New ${assetType.assetType}`} />
      <Card>
        <NewAssetControl assetType={assetType} />
      </Card>
    </div>
  );
};
