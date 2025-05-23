import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";
import { ColumnPassThroughMethodOptions } from "primereact/column";
import {
  DataTableBodyRowPassThroughMethodOptions,
  DataTablePassThroughMethodOptions,
  DataTableValueArray,
} from "primereact/datatable";

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

export const dataTableStyles: PrimeReactPTOptions = {
  datatable: {
    root: ({
      props,
    }: DataTablePassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames("relative", {
        "flex flex-col h-full":
          props.scrollable && props.scrollHeight === "flex",
      }),
    }),
    loadingOverlay: {
      className: classNames(
        "fixed w-full h-full t-0 l-0 bg-gray-100/40",
        "transition duration-200",
        "absolute flex items-center justify-center z-2",
        "dark:bg-gray-950/40" // Dark Mode
      ),
    },
    loadingIcon: { className: "w-8 h-8" },
    wrapper: ({
      props,
    }: DataTablePassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames({
        relative: props.scrollable,
        "flex flex-col grow h-full":
          props.scrollable && props.scrollHeight === "flex",
      }),
    }),
    header: ({
      props,
    }: DataTablePassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames(
        "bg-slate-50 text-slate-700 border-gray-300 font-bold p-4",
        "dark:border-blue-900/40 dark:text-white/80 dark:bg-gray-900", // Dark Mode
        props.showGridlines
          ? "border-x border-t border-b-0"
          : "border-y border-x-0"
      ),
    }),
    table: { className: "w-full border-spacing-0" },
    thead: ({
      context,
    }: DataTablePassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames({
        "bg-slate-50 top-0 z-1": context.scrollable,
      }),
    }),
    tbody: ({
      props,
      context,
    }: DataTablePassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames({
        "sticky z-1": props.frozenRow && context.scrollable,
      }),
    }),
    tfoot: ({
      context,
    }: DataTablePassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames({
        "bg-slate-50 bottom-0 z-1": context.scrollable,
      }),
    }),
    footer: {
      className: classNames(
        "bg-slate-50 text-slate-700 border-t-0 border-b border-x-0 border-gray-300 font-bold p-4",
        "dark:border-blue-900/40 dark:text-white/80 dark:bg-gray-900" // Dark Mode
      ),
    },
    column: {
      headerCell: ({ context, props }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "text-left border-0 border-b border-solid border-gray-300 dark:border-blue-900/40 font-bold",
          "transition duration-200",
          context?.size === "small"
            ? "p-2"
            : context?.size === "large"
            ? "p-5"
            : "p-4", // Size
          context.sorted
            ? "bg-blue-50 text-blue-700"
            : "bg-slate-50 text-slate-700", // Sort
          context.sorted
            ? "dark:text-white/80 dark:bg-blue-300"
            : "dark:text-white/80 dark:bg-gray-900", // Dark Mode
          {
            "sticky z-1": props.frozen, // Frozen Columns
            "border-x border-y": context?.showGridlines,
            "overflow-hidden space-nowrap border-y relative bg-clip-padding":
              context.resizable, // Resizable
          }
        ),
      }),
      headerContent: { className: "flex items-center" },
      bodyCell: ({
        props,
        context,
        state,
        parent,
      }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "text-left",
          // "border-0 border-b border-solid border-gray-300",
          context?.size === "small"
            ? "p-2"
            : context?.size === "large"
            ? "p-5"
            : "p-4", // Size
          "dark:text-white/80 dark:border-blue-900/40", // Dark Mode
          {
            "sticky bg-inherit": props && props.frozen, // Frozen Columns
            "border-x border-y": context.showGridlines,
          }
        ),
      }),
      footerCell: ({ context }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "text-left border-0 border-b border-solid border-gray-300 font-bold",
          "bg-slate-50 text-slate-700",
          "transition duration-200",
          context?.size === "small"
            ? "p-2"
            : context?.size === "large"
            ? "p-5"
            : "p-4", // Size
          "dark:text-white/80 dark:bg-gray-900 dark:border-blue-900/40", // Dark Mode
          {
            "border-x border-y": context.showGridlines,
          }
        ),
      }),
      sortIcon: ({ context }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "ml-2",
          context.sorted
            ? "text-blue-700 dark:text-white/80"
            : "text-slate-700 dark:text-white/70"
        ),
      }),
      sortBadge: {
        className: classNames(
          "flex items-center justify-center align-middle",
          "rounded-[50%] w-[1.143rem] leading-[1.143rem] ml-2",
          "text-blue-700 bg-blue-50",
          "dark:text-white/80 dark:bg-blue-400" // Dark Mode
        ),
      },
      columnFilter: { className: "inline-flex items-center ml-auto" },
      filterOverlay: {
        className: classNames(
          "bg-white text-gray-600 border-0 rounded-md min-w-[12.5rem]",
          "dark:bg-gray-900 dark:border-blue-900/40 dark:text-white/80" //Dark Mode
        ),
      },
      filterMatchModeDropdown: {
        root: "min-[0px]:flex mb-2",
      },
      filterRowItems: { className: "m-0 p-0 py-3 list-none " },
      filterRowItem: ({ context }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "m-0 py-3 px-5 bg-transparent",
          "transition duration-200",
          context?.highlighted
            ? "text-blue-700 bg-blue-100 dark:text-white/80 dark:bg-blue-300"
            : "text-gray-600 bg-transparent dark:text-white/80 dark:bg-transparent"
        ),
      }),
      filterOperator: {
        className: classNames(
          "px-5 py-3 border-b border-solid border-gray-300 text-slate-700 bg-slate-50 rounded-t-md",
          "dark:border-blue-900/40 dark:text-white/80 dark:bg-gray-900" // Dark Mode
        ),
      },
      filterOperatorDropdown: {
        root: "min-[0px]:flex",
      },
      filterConstraint: {
        className:
          "p-5 border-b border-solid border-gray-300 dark:border-blue-900/40",
      },
      filterAddRule: { className: "py-3 px-5" },
      filterAddRuleButton: {
        root: "justify-center w-full min-[0px]:text-sm",
        label: "flex-auto grow-0",
        icon: "mr-2",
      },
      filterRemoveButton: {
        root: "ml-2",
        label: "grow-0",
      },
      filterButtonbar: { classNames: "flex items-center justify-between p-5" },
      filterClearButton: {
        root: "w-auto min-[0px]:text-sm border-blue-500 text-blue-500 px-4 py-3",
      },
      filterApplyButton: {
        root: "w-auto min-[0px]:text-sm px-4 py-3",
      },
      filterMenuButton: ({ context }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "inline-flex justify-center items-center cursor-pointer no-underline overflow-hidden relative ml-2",
          "w-8 h-8 rounded-[50%]",
          "transition duration-200",
          "hover:text-slate-700 hover:bg-gray-300/20", // Hover
          "focus:outline-0 focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)]", // Focus
          "dark:text-white/70 dark:hover:text-white/80 dark:bg-gray-900", // Dark Mode
          {
            "bg-blue-50 text-blue-700": context.active,
          }
        ),
      }),
      headerFilterClearButton: ({
        context,
      }: ColumnPassThroughMethodOptions) => ({
        className: classNames(
          "inline-flex justify-center items-center cursor-pointer no-underline overflow-hidden relative",
          "text-left bg-transparent m-0 p-0 border-none select-none ml-2",
          {
            invisible: !context.hidden,
          }
        ),
      }),
      columnResizer: {
        className:
          "block absolute top-0 right-0 m-0 w-2 h-full p-0 cursor-col-resize border border-transparent",
      },
      rowReorderIcon: { classNames: "cursor-move" },
      rowEditorInitButton: {
        className: classNames(
          "inline-flex items-center justify-center overflow-hidden relative",
          "text-left cursor-pointer select-none",
          "w-8 h-8 border-0 rounded-[50%]",
          "transition duration-200",
          "text-slate-700 border-transparent",
          "focus:outline-0 focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)]", //Focus
          "hover:text-slate-700 hover:bg-gray-300/20", //Hover
          "dark:text-white/70" // Dark Mode
        ),
      },
      rowEditorSaveButton: {
        className: classNames(
          "inline-flex items-center justify-center overflow-hidden relative",
          "text-left cursor-pointer select-none",
          "w-8 h-8 border-0 rounded-[50%]",
          "transition duration-200",
          "text-slate-700 border-transparent",
          "focus:outline-0 focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)]", //Focus
          "hover:text-slate-700 hover:bg-gray-300/20", //Hover
          "dark:text-white/70" // Dark Mode
        ),
      },
      rowEditorCancelButton: {
        className: classNames(
          "inline-flex items-center justify-center overflow-hidden relative",
          "text-left cursor-pointer select-none",
          "w-8 h-8 border-0 rounded-[50%]",
          "transition duration-200",
          "text-slate-700 border-transparent",
          "focus:outline-0 focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)]", //Focus
          "hover:text-slate-700 hover:bg-gray-300/20", //Hover
          "dark:text-white/70" // Dark Mode
        ),
      },
      // headerCheckbox: ({ context }) => ({
      //   className: classNames(
      //     "flex items-center justify-center",
      //     "border-2 w-6 h-6 text-gray-600 rounded-lg transition-colors duration-200",
      //     context.checked
      //       ? "border-blue-500 bg-blue-500 dark:border-blue-400 dark:bg-blue-400"
      //       : "border-gray-300 bg-white dark:border-blue-900/40 dark:bg-gray-900",
      //     {
      //       "hover:border-blue-500 dark:hover:border-blue-400 focus:outline-hidden focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)] dark:focus:shadow-[inset_0_0_0_0.2rem_rgba(147,197,253,0.5)]":
      //         !context.disabled,
      //       "cursor-default opacity-60": context.disabled,
      //     }
      //   ),
      // }),
      // headerCheckboxIcon: {
      //   classNames:
      //     "w-4 h-4 transition-all duration-200 text-white text-base dark:text-gray-900",
      // },
      // checkboxwrapper: {
      //   className: classNames(
      //     "cursor-pointer inline-flex relative select-none align-bottom",
      //     "w-6 h-6"
      //   ),
      // },
      // checkbox: ({ context }) => ({
      //   className: classNames(
      //     "flex items-center justify-center",
      //     "border-2 w-6 h-6 text-gray-600 rounded-lg transition-colors duration-200",
      //     context.checked
      //       ? "border-blue-500 bg-blue-500 dark:border-blue-400 dark:bg-blue-400"
      //       : "border-gray-300 bg-white dark:border-blue-900/40 dark:bg-gray-900",
      //     {
      //       "hover:border-blue-500 dark:hover:border-blue-400 focus:outline-hidden focus:outline-offset-0 focus:shadow-[0_0_0_0.2rem_rgba(191,219,254,1)] dark:focus:shadow-[inset_0_0_0_0.2rem_rgba(147,197,253,0.5)]":
      //         !context.disabled,
      //       "cursor-default opacity-60": context.disabled,
      //     }
      //   ),
      // }),
      // checkboxicon:
      //   "w-4 h-4 transition-all duration-200 text-white text-base dark:text-gray-900",
      //transition: TRANSITIONS.overlay,
    },
    bodyRow: ({
      context,
    }: DataTableBodyRowPassThroughMethodOptions<DataTableValueArray>) => ({
      className: classNames(
        context.selected
          ? "bg-blue-50 text-blue-700 dark:bg-blue-300"
          : "bg-white text-gray-600 dark:bg-gray-900",
        context.stripedRows
          ? context.index % 2 === 0
            ? "bg-white text-gray-600 dark:bg-gray-900"
            : "bg-blue-50/50 text-gray-600 dark:bg-gray-950"
          : "",
        "transition duration-200",
        "focus:outline focus:outline-[0.15rem] focus:outline-blue-200 focus:outline-offset-[-0.15rem]", // Focus
        "dark:text-white/80 dark:focus:outline dark:focus:outline-[0.15rem] dark:focus:outline-blue-300 dark:focus:outline-offset-[-0.15rem]", // Dark Mode
        {
          "cursor-pointer": context.selectable,
          "hover:bg-gray-300/20 hover:text-gray-600":
            context.selectable && !context.selected, // Hover
        }
      ),
    }),
    rowExpansion: {
      className: "bg-white text-gray-600 dark:bg-gray-900 dark:text-white/80",
    },
    rowGroupHeader: {
      className: classNames(
        "sticky z-1",
        "bg-white text-gray-600",
        "transition duration-200"
      ),
    },
    rowGroupFooter: {
      className: classNames(
        "sticky z-1",
        "bg-white text-gray-600",
        "transition duration-200"
      ),
    },
    rowGroupToggler: {
      className: classNames(
        "text-left m-0 p-0 cursor-pointer select-none",
        "inline-flex items-center justify-center overflow-hidden relative",
        "w-8 h-8 text-gray-500 border-0 bg-transparent rounded-[50%]",
        "transition duration-200",
        "dark:text-white/70" // Dark Mode
      ),
    },
    rowGroupTogglerIcon: { className: "inline-block w-4 h-4" },
    resizeHelper: {
      className: "absolute hidden w-px z-10 bg-blue-500 dark:bg-blue-300",
    },
  },
};
