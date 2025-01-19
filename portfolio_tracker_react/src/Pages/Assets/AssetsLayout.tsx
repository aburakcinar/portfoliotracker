import { Outlet } from "react-router";

export const AssetsLayout: React.FC = () => {
  return (
    <div className="pt-16 h-auto">
      <div className="flex w-full justify-center ">
        <Outlet />
      </div>
    </div>
  );
};
