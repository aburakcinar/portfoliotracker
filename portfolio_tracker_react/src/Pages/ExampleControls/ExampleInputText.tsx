import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import React from "react";

export const ExampleInputText: React.FC = () => {
  return (
    <>
      <h3 className="text-2xl text-green m-10 my-4">InputText</h3>
      <Card className="m-10">
        <InputText className="w-20 h-10" />
      </Card>
    </>
  );
};
