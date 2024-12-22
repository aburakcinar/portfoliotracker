import classNames from "classnames";
import { PrimeReactPTOptions } from "primereact/api";

export const cardStyles: PrimeReactPTOptions = {
  card: {
    root: {
      className: classNames(
        "bg-white text-gray-700 shadow-md rounded-md", // Background, text color, box shadow, and border radius.
        "dark:bg-gray-900 dark:text-white " //dark
      ),
    },
    body: { className: "p-5" }, // Padding.
    title: { className: "text-2xl font-bold mb-2" }, // Font size, font weight, and margin bottom.
    subTitle: {
      className: classNames(
        "font-normal mb-2 text-gray-600", // Font weight, margin bottom, and text color.
        "dark:text-white/60 " //dark
      ),
    },
    content: { className: "py-5" }, // Vertical padding.
    footer: { className: "pt-5" }, // Top padding.
  },
};
