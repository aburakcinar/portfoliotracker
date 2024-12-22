import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { SelectButtonPassThroughMethodOptions } from "primereact/selectbutton";

export const selectButtonStyles: PrimeReactPTOptions = {
  selectbutton: {
    root: ({ props }: SelectButtonPassThroughMethodOptions) => ({
      className: classNames({
        "opacity-60 select-none pointer-events-none cursor-default":
          props.disabled,
      }),
    }),
    button: ({ context }: SelectButtonPassThroughMethodOptions) => ({
      className: classNames(
        "inline-flex cursor-pointer select-none items-center align-bottom text-center overflow-hidden relative",
        "px-2 py-2",
        "transition duration-200 border border-r-0",
        "focus:outline-none focus:outline-offset-0 ",
        {
          "bg-white dark:bg-gray-900 text-gray-700 dark:text-white/80 border-gray-300 dark:border-blue-900/40 hover:bg-gray-50 dark:hover:bg-gray-800/80 ":
            !context.selected,
          "bg-blue-500 border-blue-500 text-white hover:bg-blue-600":
            context.selected,
          "opacity-60 select-none pointer-events-none cursor-default":
            context.disabled,
        }
      ),
    }),
    label: { className: "font-bold" },
  },
};
