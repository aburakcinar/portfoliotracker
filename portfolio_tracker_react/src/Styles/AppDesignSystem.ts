import { PrimeReactPTOptions } from "primereact/api";
import { dropdownStyles } from "./DropdownStyles";
import { selectButtonStyles } from "./SelectButtonStyles";
import { cardStyles } from "./CardStyles";
import { inputTextStyles } from "./InputTextStyles";
import { dialogStyles } from "./DialogStyles";
import { autoCompleteStyle } from "./AutoCompleteStyles";
import { calendarStyles } from "./CalendarStyles";
import { inputNumberStyles } from "./InputNumberStyles";

export const appDesignSystem: PrimeReactPTOptions = {
  ...dropdownStyles,
  ...selectButtonStyles,
  ...cardStyles,
  ...inputTextStyles,
  ...dialogStyles,
  ...autoCompleteStyle,
  ...calendarStyles,
  ...inputNumberStyles,
};
