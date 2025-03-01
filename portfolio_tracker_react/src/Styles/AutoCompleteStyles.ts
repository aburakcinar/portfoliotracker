import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { AutoCompletePassThroughMethodOptions } from "primereact/autocomplete";

const TRANSITIONS = {
  overlay: {
    timeout: 150,
    classNames: {
      enter: "opacity-0 scale-75",
      enterActive:
        "opacity-100 scale-100! transition-transform transition-opacity duration-150 ease-in",
      exit: "opacity-100",
      exitActive: "opacity-0! transition-opacity duration-150 ease-linear",
    },
  },
};

export const autoCompleteStyle: PrimeReactPTOptions = {
  autocomplete: {
    root: ({ props }: AutoCompletePassThroughMethodOptions) => ({
      className: classNames(
        "relative flex",
        "border border-green dark:border-green",
        "hover:border-green/60 dark:hover:border-green/60",
        "dark:bg-nav",
        {
          "opacity-60 select-none pointer-events-none cursor-default":
            props.disabled,
        },
        { "w-full": props.multiple }
      ),
    }),
    container: {
      className: classNames(
        "m-0 list-none cursor-text overflow-hidden flex items-center flex-wrap ",
        "px-3 py-2 gap-2",
        "font-sans text-base text-gray-700 dark:text-white/80 bg-white dark:bg-nav",
        "border-1 border-green dark:border-green  ",
        "hover:border-blue-700 focus:outline-hidden "
      ),
    },
    inputToken: {
      className: classNames(
        "py-0.375rem px-0",
        "flex-1 inline-flex",
        "autocomplete"
      ),
    },
    input: {
      root: {
        className: classNames(
          "grow m-0 border-0",
          "focus-visible:outline-hidden focus-visible:outline-offset-0 ",
          "transition-colors duration-200 appearance-none ",
          { "border-r-0 dark:border-r-0": true }
        ),
      },
    },
    token: {
      className: classNames(
        "py-1 px-2 mr-2 bg-gray-300 dark:bg-highlight text-gray-700 dark:text-green rounded-xs",
        "cursor-default inline-flex items-center"
      ),
    },
    dropdownButton: {
      root: {
        className: classNames(
          "grow-0 bg-green dark:bg-green",
          "hover:bg-green/60 dark:hover:bg-green/60",
          "border-green dark:border-green",
          "hover:border-green dark:hover:border-green"
        ),
      },
    },
    panel: {
      className: classNames(
        "bg-white text-gray-700 border-0 shadow-lg",
        "max-h-[200px] overflow-auto",
        "dark:bg-gray-900 dark:text-white/80"
      ),
    },
    list: { className: "py-3 list-none m-0" },
    item: ({ context }: AutoCompletePassThroughMethodOptions) => ({
      className: classNames(
        "cursor-pointer font-normal overflow-hidden relative whitespace-nowrap",
        "m-0 p-3 border-0  transition-shadow duration-200 rounded-none",
        {
          "text-gray-700 hover:text-gray-700 hover:bg-gray-200 dark:text-white/80 dark:hover:bg-gray-800":
            !context.selected,
          "bg-gray-300 text-gray-700 dark:text-white/80 dark:bg-gray-800/90 hover:text-gray-700 hover:bg-gray-200 dark:text-white/80 dark:hover:bg-gray-800":
            context.disabled && !context.selected,
          "bg-blue-100 text-blue-700 dark:bg-blue-400 dark:text-white/80":
            context.selected,
          "bg-blue-50 text-blue-700 dark:bg-blue-300 dark:text-white/80":
            context.selected,
        }
      ),
    }),
    itemGroup: {
      className: classNames(
        "m-0 p-3 text-gray-800 bg-white font-bold",
        "dark:bg-gray-900 dark:text-white/80",
        "cursor-auto"
      ),
    },
    transition: TRANSITIONS.overlay,
  },
};
