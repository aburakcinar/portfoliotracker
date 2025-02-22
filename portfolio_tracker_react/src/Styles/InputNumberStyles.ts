import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { InputNumberPassThroughMethodOptions } from "primereact/inputnumber";

export const inputNumberStyles: PrimeReactPTOptions = {
  inputnumber: {
    root: {
      className: classNames(
        "flex flex-row",
        "border border-green", // Border
        "dark:border-green dark:bg-nav", // Dark
        "hover:border-green/60 ", // Hover
        "dark:hover:border-green/60" // Hover Dark
      ),
    },
    input: {
      root: ({ props }: InputNumberPassThroughMethodOptions) => ({
        className: classNames(
          "grow border-0 w-full h-full !items-center !justify-center",
          "rounded-none focus-visible:outline-none order-2",
          "bg-transparent dark:bg-transparent" // Get Bg from parent
        ),
      }),
    },
    buttonGroup: ({ props }: InputNumberPassThroughMethodOptions) => ({
      className: classNames({
        "flex flex-col":
          props.showButtons && props.buttonLayout === "horizontal",
      }),
    }),
    incrementButton: ({ props }: InputNumberPassThroughMethodOptions) => ({
      className: classNames(
        "flex !items-center !justify-center text-gray-400",
        {
          "grow-0 order-1":
            props.showButtons && props.buttonLayout === "horizontal",
        },
        "dark:bg-nav",
        {
          "!p-0 flex-1 w-[3rem]":
            props.showButtons && props.buttonLayout === "stacked",
        }
      ),
    }),
    decrementButton: ({ props }: InputNumberPassThroughMethodOptions) => ({
      className: classNames(
        "flex !items-center !justify-center text-gray-400",
        {
          "grow-0 order-3":
            props.showButtons && props.buttonLayout === "horizontal",
        },
        {
          "!p-0 flex-1 w-[3rem]":
            props.showButtons && props.buttonLayout == "stacked",
        }
      ),
    }),
  },
};
