import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { CalendarPassThroughMethodOptions } from "primereact/calendar";

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

export const calendarStyles: PrimeReactPTOptions = {
  calendar: {
    root: ({ props }: CalendarPassThroughMethodOptions) => ({
      className: classNames("flex max-w-full relative", {
        "opacity-60 select-none pointer-events-none cursor-default":
          props.disabled,
      }),
    }),
    input: ({ props }: CalendarPassThroughMethodOptions) => ({
      root: {
        className: classNames(
          "grow font-sans text-base text-gray-600 dark:text-white/80 bg-white dark:bg-gray-900 p-2 border",
          "border-gray-300 dark:border-gray-900/40 transition-colors duration-200 appearance-none",
          "hover:border-[#28ebcf]",
          {
            "border-r-0 ": props.showIcon,
          }
        ),
      },
    }),
    dropdownButton: {
      root: ({ props }: CalendarPassThroughMethodOptions) => ({
        className: classNames(
          "grow-0 bg-green dark:bg-green hover:bg-green/80 dark:hover:bg-green/80 text-nav"
        ),
      }),
    },
    panel: ({ props }: CalendarPassThroughMethodOptions) => ({
      className: classNames("bg-white dark:bg-gray-900", {
        "shadow-md border-0 absolute": !props.inline,
        "inline-block overflow-x-auto border border-gray-300 dark:border-gray-900/40 p-2 ":
          props.inline,
      }),
    }),
    header: {
      className: classNames(
        "flex items-center justify-between",
        "p-2 text-gray-700 dark:text-white/80 bg-white dark:bg-gray-900 font-semibold m-0 border-b border-gray-300",
        "dark:border-gray-900/40 rounded-t-lg"
      ),
    },
    previousButton: {
      className: classNames(
        "flex items-center justify-center cursor-pointer overflow-hidden relative",
        "size-8 text-gray-600 dark:text-white/70 border-0 bg-transparent rounded-full transition-colors duration-200",
        "ease-in-out",
        "hover:text-gray-700 dark:hover:text-white/80 hover:border-transparent hover:bg-gray-200 dark:hover:bg-gray-800/80"
      ),
    },
    title: { className: "leading-8 mx-auto" },
    monthTitle: {
      className: classNames(
        "text-gray-700 dark:text-white/80 transition duration-200 font-semibold p-2",
        "mr-2",
        "hover:text-[#28ebcf]"
      ),
    },
    yearTitle: {
      className: classNames(
        "text-gray-700 dark:text-white/80 transition duration-200 font-semibold p-2",
        "hover:text-[#28ebcf]"
      ),
    },
    nextButton: {
      className: classNames(
        "flex items-center justify-center cursor-pointer overflow-hidden relative",
        "w-8 h-8 text-gray-600 dark:text-white/70 border-0 bg-transparent rounded-full transition-colors duration-200 ease-in-out",
        "hover:text-gray-700 dark:hover:text-white/80 hover:border-transparent hover:bg-gray-200 dark:hover:bg-gray-800/80 "
      ),
    },
    table: {
      className: classNames("border-collapse w-full", "my-2"),
    },
    tableHeaderCell: { className: "p-2" },
    weekDay: { className: "text-gray-600 dark:text-white/70" },
    day: { className: "p-2" },
    dayLabel: ({ context }: CalendarPassThroughMethodOptions) => ({
      className: classNames(
        "w-10 h-10 rounded-full transition-shadow duration-200 border-transparent border",
        "flex items-center justify-center mx-auto overflow-hidden relative",
        "focus:outline-hidden focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(40,235,207,0.4)]",
        "dark:focus:shadow-[0_0_0_0.2rem_rgba(40,235,207,0.4)]",
        {
          "opacity-60 cursor-default": context.disabled,
          "cursor-pointer": !context.disabled,
        },
        {
          "text-gray-600 dark:text-white/70 bg-transprent hover:bg-gray-200 dark:hover:bg-gray-800/80":
            !context.selected && !context.disabled,
          "text-[#28ebcf] bg-[#28ebcf]/10 hover:bg-[#28ebcf]/20":
            context.selected && !context.disabled,
        }
      ),
    }),
    monthPicker: { className: "my-2" },
    month: ({ context }: CalendarPassThroughMethodOptions) => ({
      className: classNames(
        "w-1/3 inline-flex items-center justify-center cursor-pointer overflow-hidden relative",
        "p-2 transition-shadow duration-200 rounded-lg",
        "focus:outline-hidden focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(40,235,207,0.4)]",
        "dark:focus:shadow-[0_0_0_0.2rem_rgba(40,235,207,0.4)]",
        {
          "text-gray-600 dark:text-white/70 bg-transprent hover:bg-gray-200 dark:hover:bg-gray-800/80":
            !context.selected && !context.disabled,
          "text-[#28ebcf] bg-[#28ebcf]/10 hover:bg-[#28ebcf]/20":
            context.selected && !context.disabled,
        }
      ),
    }),
    yearPicker: {
      className: classNames("my-2"),
    },
    year: ({ context }: CalendarPassThroughMethodOptions) => ({
      className: classNames(
        "w-1/2 inline-flex items-center justify-center cursor-pointer overflow-hidden relative",
        "p-2 transition-shadow duration-200 rounded-lg",
        "focus:outline-hidden focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(40,235,207,0.4)]",
        "dark:focus:shadow-[0_0_0_0.2rem_rgba(40,235,207,0.4)]",
        {
          "text-gray-600 dark:text-white/70 bg-transprent hover:bg-gray-200 dark:hover:bg-gray-800/80":
            !context.selected && !context.disabled,
          "text-[#28ebcf] bg-[#28ebcf]/10 hover:bg-[#28ebcf]/20":
            context.selected && !context.disabled,
        }
      ),
    }),
    timePicker: {
      className: classNames(
        "flex justify-center items-center",
        "border-t-1 border-solid border-gray-300 p-2"
      ),
    },
    separatorContainer: { className: "flex items-center flex-col px-2" },
    separator: { className: "text-xl" },
    hourPicker: { className: "flex items-center flex-col px-2" },
    minutePicker: { className: "flex items-center flex-col px-2" },
    ampmPicker: { className: "flex items-center flex-col px-2" },
    incrementButton: {
      className: classNames(
        "flex items-center justify-center cursor-pointer overflow-hidden relative",
        "size-8 text-gray-600 dark:text-white/70 border-0 bg-transparent rounded-full transition-colors duration-200 ease-in-out",
        "hover:text-gray-700 dark:hover:text-white/80 hover:border-transparent hover:bg-gray-200 dark:hover:bg-gray-800/80 "
      ),
    },
    decrementButton: {
      className: classNames(
        "flex items-center justify-center cursor-pointer overflow-hidden relative",
        "size-8 text-gray-600 dark:text-white/70 border-0 bg-transparent rounded-full transition-colors duration-200 ease-in-out",
        "hover:text-gray-700 dark:hover:text-white/80 hover:border-transparent hover:bg-gray-200 dark:hover:bg-gray-800/80 "
      ),
    },
    groupContainer: { className: "flex" },
    group: {
      className: classNames(
        "flex-1",
        "border-l border-gray-300 pr-0.5 pl-0.5 pt-0 pb-0",
        "first:pl-0 first:border-l-0"
      ),
    },
    transition: TRANSITIONS.overlay,
  },
};
