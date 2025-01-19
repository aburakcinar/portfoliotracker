import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { InputTextareaPassThroughMethodOptions } from "primereact/inputtextarea";

export const inputTextareaStyles: PrimeReactPTOptions = {
  inputtextarea: {
    root: ({ context }: InputTextareaPassThroughMethodOptions) => ({
      className: classNames(
        "m-0",
        "font-sans text-base text-gray-600 dark:text-white/80", // Font
        "bg-white dark:bg-nav", // BackGround
        "p-3 border border-green dark:border-green transition-colors duration-200 appearance-none",
        "hover:border-green/60 dark:hover:border-green/60", // Hover Border
        "focus:outline-none focus:outline-offset-0",
        {
          "opacity-60 select-none pointer-events-none cursor-default":
            context.disabled,
        }
      ),
    }),
  },
};
