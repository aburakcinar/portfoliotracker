import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";

export const breadCrumbStyles: PrimeReactPTOptions = {
  breadcrumb: {
    root: {
      className: classNames("overflow-x-auto", "   p-4"),
    },
    menu: { className: "m-0 p-0 list-none flex items-center flex-nowrap" },
    action: {
      className: classNames(
        "text-decoration-none flex items-center",
        "transition-shadow duration-200 rounded-md text-gray-600 dark:text-white/70",
        "focus:outline-none focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)] dark:focus:shadow-[0_0_0_0.2rem_rgba(147,197,253,0.5)]"
      ),
    },
    icon: { className: "text-green dark:text-white/70" },
    separator: {
      className: classNames(
        "mx-2 text-gray-600 dark:text-white/80",
        "flex items-center"
      ),
    },
  },
};
