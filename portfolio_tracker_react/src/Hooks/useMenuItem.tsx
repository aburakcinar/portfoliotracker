import React, { DependencyList, useEffect } from "react";
import { addMenuItem, IMenuItem, removeMenuItem } from "../Store";
import { useAppDispatch } from "../Store/RootState";

export const useMenuItem = (
  menuItem: IMenuItem | IMenuItem[],
  dependencies: DependencyList
): void => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(addMenuItem(menuItem));

    return () => {
      dispatch(removeMenuItem(menuItem));
    };
  }, dependencies);
};
