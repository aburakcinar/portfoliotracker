import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { InputTextPassThroughMethodOptions } from "primereact/inputtext";

export const inputTextStyles: PrimeReactPTOptions = {
  inputtext: {
    root: ({ props, context }: InputTextPassThroughMethodOptions) => ({
      className: classNames(
        "m-0",
        "font-sans text-gray-600 dark:text-white/80 bg-white",
        "border border-gray-900 dark:border-green dark:bg-nav",
        "hover:border-green/60 dark:hover:border-green/60", // Hover Border
        "focus:outline-none focus-visible:outline-none",
        "transition-colors duration-200 appearance-none ",
        {
          "focus:border-gray-500": !context.disabled,
          "hover:border-blue-500": !props.invalid && !context.disabled,
          "opacity-60 select-none pointer-events-none cursor-default":
            context.disabled,
          "border-gray-300 dark:border-blue-900/40": !props.invalid,
          "border-red-500 hover:border-red-500/80 focus:border-red-500":
            props.invalid && !context.disabled,
          "border-red-500/50": props.invalid && context.disabled,
        },
        {
          "text-lg px-4 py-4": props.size === "large",
          "text-xs px-2 py-2": props.size === "small",
          "p-1 text-base": !props.size || typeof props.size === "number",
        }
        // {
        //   "pl-8": context.iconPosition === "left",
        //   "pr-8": props.iconPosition === "right",
        // }
      ),
    }),
  },
};
