import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { ButtonPassThroughOptions } from "primereact/button";

const acceptButton: ButtonPassThroughOptions = {};

export const confirmPopupStyles: PrimeReactPTOptions = {
  confirmpopup: {
    root: {
      className: classNames(
        "bg-white text-gray-700 border-0 rounded-md shadow-lg",
        "z-40 transform origin-center",
        "mt-3 absolute left-0 top-0",
        "before:absolute before:w-0 before:-top-3 before:h-0 before:border-transparent before:border-solid before:ml-6 before:border-x-[0.75rem] before:border-b-[0.75rem] before:border-t-0 before:border-b-white dark:before:border-b-gray-900",
        "dark:border dark:border-blue-900/40 dark:bg-gray-900  dark:text-white/80"
      ),
    },
    acceptButton: {
      root: {
        className: classNames(
          "text-sm",
          "border-0", // Border
          "bg-red-500 dark:bg-red-500 hover:bg-red-600 dark:hover:bg-red-600", // Background
          "py-1 px-0 m-1", // Padding | Margin
          "shadow-none dark:shadow-none focus:shadow-none dark:focus:shadow-none" // Shadow
        ),
      },
    },
    rejectButton: {
      root: {
        className: classNames(
          "text-sm",
          "border-0", // Border
          "bg-red-500 dark:bg-slate-500 hover:bg-slate-600 dark:hover:bg-slate-600", // Background
          "py-1 px-0 m-1", // Padding | Margin
          "shadow-none dark:shadow-none focus:shadow-none dark:focus:shadow-none" // Shadow
        ),
      },
    },

    content: { className: "p-5 items-center flex" },
    icon: { className: "text-2xl" },
    message: { className: "ml-4" },
    footer: { className: "text-right px-5 py-5 pt-0 " },
  },
};
