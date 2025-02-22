import React from "react";
import { useAppSelector } from "../Store/RootState";
import { IMenuItem } from "../Store";
import { Link } from "react-router";
import { HomeModernIcon } from "@heroicons/react/24/outline";

export const MenuContainer: React.FC = () => {
  const menus = useAppSelector((x) => x.menu.list);

  return (
    <ul className="text-white ">
      {menus.map((menu) => (
        <MenuItem key={menu.id} item={menu} />
      ))}
    </ul>
  );
};

interface IMenuItemProps {
  item: IMenuItem;
}

const MenuItem: React.FC<IMenuItemProps> = (props) => {
  const { item } = props;

  const renderSubMenu = () => {
    const children = item.children?.filter((x) => x.visible);

    if (children && children.length > 0) {
      return (
        <div className="flex">
          <div className="grow-0 w-8">&nbsp;</div>
          <div className="grow">
            <ul className="text-white">
              {children.map((item) => (
                <MenuItem key={item.id} item={item} />
              ))}
            </ul>
          </div>
        </div>
      );
    }

    return null;
  };

  if (item.visible === false) {
    return null;
  }

  return (
    <>
      <Link to={item.link}>
        <li className="p-2 hover:bg-highlight">
          <div className="flex text-green">
            <span className="grow py-1 text-xl">{item.text}</span>
          </div>
        </li>
      </Link>
      {renderSubMenu()}
    </>
  );
};
