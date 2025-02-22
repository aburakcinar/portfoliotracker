import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";

export const overlayPanelStyles: PrimeReactPTOptions = {
  overlaypanel: {
    root: {
      className: classNames(
        "bg-white text-gray-700  shadow-lg",
        "z-40 transform origin-center",
        "absolute left-0 top-0 ",
        "before:absolute before:w-0 before:-top-3 before:h-0 before:border-transparent before:border-solid before:ml-6 before:border-x-[0.75rem] before:border-b-[0.75rem] before:border-t-0 before:border-b-white dark:before:border-b-gray-900",
        "dark:border dark:border-blue-900/40 dark:bg-gray-900  dark:text-white/80"
      ),
    },
    closeButton: {
      classNames:
        "flex items-center justify-center overflow-hidden absolute top-0 right-0 w-6 h-6",
    },
    content: { className: "items-center flex" },
  },
};
