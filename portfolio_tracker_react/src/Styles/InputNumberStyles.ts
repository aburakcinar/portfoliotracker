import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { InputNumberPassThroughMethodOptions } from "primereact/inputnumber";

export const inputNumberStyles: PrimeReactPTOptions = {
  inputnumber: {
    root: {
      className:
        "inline-flex border border-gray-900 dark:border-green dark:bg-nav ",
    },
    input: {
      root: ({ props }: InputNumberPassThroughMethodOptions) => ({
        className: classNames(
          "grow order-2 border-0 h-8 !items-center !justify-center",
          "rounded-none focus-visible:outline-none",
          {
            " ": props.showButtons && props.buttonLayout == "stacked",
          }
        ),
      }),
    },
    buttonGroup: ({ props }: InputNumberPassThroughMethodOptions) => ({
      className: classNames({
        "flex flex-col": props.showButtons && props.buttonLayout == "stacked",
      }),
    }),
    incrementButton: ({ props }: InputNumberPassThroughMethodOptions) => ({
      className: classNames(
        "flex !items-center !justify-center order-1 text-gray-400",
        "dark:bg-nav",
        {
          "!p-0 flex-1 w-[3rem]":
            props.showButtons && props.buttonLayout == "stacked",
        }
      ),
    }),
    decrementButton: ({ props }: InputNumberPassThroughMethodOptions) => ({
      className: classNames(
        "flex !items-center !justify-center order-3 text-gray-400",
        "",
        {
          "!p-0 flex-1 w-[3rem]":
            props.showButtons && props.buttonLayout == "stacked",
        }
      ),
    }),
  },
};
