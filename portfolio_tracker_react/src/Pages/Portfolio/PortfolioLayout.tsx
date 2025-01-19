import { Outlet } from "react-router";

export const PortfolioLayout: React.FC = () => {
  return (
    <div className="pt-16 h-auto flex w-full justify-center">
      <Outlet />
    </div>
  );
};
