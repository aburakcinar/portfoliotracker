import React, { useEffect, useState } from "react";
import { BreadCrumb } from "primereact/breadcrumb";
import { useNavigate, useLocation } from "react-router";
import { generateBreadcrumbItems } from "../Tools/Navigation";
import { MenuItem } from "primereact/menuitem";
import { useAppDispatch, useAppSelector } from "../Store/RootState";
import { HomeIcon } from "@heroicons/react/24/outline";
import { generateFlattenList } from "../Store";

export const AppBreadCrumb: React.FC = () => {
  const location = useLocation(); // Provides the current path
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const menuItemList = useAppSelector((x) => x.menu.flattenList);

  useEffect(() => {
    dispatch(generateFlattenList());
  }, []);

  const menuItems = generateBreadcrumbItems(
    menuItemList,
    location.pathname,
    navigate
  );

  const homeMenuItem: MenuItem = {
    label: "Home",
    icon: () => <HomeIcon className="size-5 m-1" />,
    url: "/",
  };

  return <BreadCrumb className="fixed" model={menuItems} home={homeMenuItem} />;
};
