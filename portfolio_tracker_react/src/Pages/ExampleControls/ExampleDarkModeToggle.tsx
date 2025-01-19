import React from "react";
import { Card } from "primereact/card";
import { DarkModeToggle } from "../../Controls/DarkModeToggle";

export const ExampleDarkModeToggle: React.FC = () => {
  return (
    <>
      <h3 className="text-2xl text-green m-10 my-4">Dark Mode Toggle</h3>
      <Card className="m-10">
        <DarkModeToggle />
      </Card>
    </>
  );
};
