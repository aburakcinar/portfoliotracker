import { MenuItem } from "primereact/menuitem";
import { IMenuItem } from "../Store";

export const generateBreadcrumbItems = (
  menuItemList: IMenuItem[],
  pathname: string,
  navigate: (path: string) => void
): MenuItem[] => {
  const pathSegments = pathname.split("/").filter((segment) => segment);

  const findLabel = (url: string): string | undefined => {
    return menuItemList.find((x) => x.link === url)?.text;
  };

  return pathSegments.map((segment, index) => {
    const url = `/${pathSegments.slice(0, index + 1).join("/")}`;

    return {
      label: findLabel(url) || segment, // Use mapped label if available
      command: () => navigate(url),
    };
  });
};

export const flattenMenuItem = (
  items: IMenuItem[] | undefined
): IMenuItem[] => {
  const result: IMenuItem[] = [];

  if (!items) {
    return result;
  }

  for (const menu of items) {
    result.push(menu);

    if (menu.children) {
      result.push(...menu.children);
    }
  }

  return result;
};
