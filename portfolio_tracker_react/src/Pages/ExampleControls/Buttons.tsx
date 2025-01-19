import { Button } from "primereact/button";
import { Card } from "primereact/card";
import React from "react";

export const Buttons: React.FC = () => {
  return (
    <>
      <h3 className="text-2xl text-green m-2 mb-10">Buttons</h3>

      <div className="dark:text-green text-xl m-2">Default Button</div>
      <Card className="m-2 flex justify-center">
        <Button>Button</Button>
      </Card>

      <div className="dark:text-green text-xl m-2">Secondary Button</div>
      <Card className="m-2 flex justify-center">
        <Button severity="secondary">Button</Button>
      </Card>

      <div className="dark:text-green text-xl m-2">Success Button</div>
      <Card className="m-2 flex justify-center">
        <Button severity="success">Button</Button>
      </Card>

      <div className="dark:text-green text-xl m-2">Warning Button</div>
      <Card className="m-2 flex justify-center">
        <Button severity="warning">Button</Button>
      </Card>

      <div className="dark:text-green text-xl m-2">Danger Button</div>
      <Card className="m-2 flex justify-center">
        <Button severity="danger">Button</Button>
      </Card>
    </>
  );
};
