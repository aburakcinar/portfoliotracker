import { PrimeReactPTOptions } from "primereact/api";
import { dropdownStyles } from "./DropdownStyles";
import { selectButtonStyles } from "./SelectButtonStyles";
import { cardStyles } from "./CardStyles";
import { inputTextStyles } from "./InputTextStyles";
import { dialogStyles } from "./DialogStyles";
import { autoCompleteStyle } from "./AutoCompleteStyles";
import { calendarStyles } from "./CalendarStyles";
import { inputNumberStyles } from "./InputNumberStyles";
import { dataTableStyles } from "./DataTableStyles";
import { contextMenuStyles } from "./ContextMenuStyles";
import { buttonStyles } from "./ButtonStyles";
import { confirmPopupStyles } from "./ConfirmPopupStyles";
import { paginatorStyles } from "./PaginatorStyles";

export const appDesignSystem: PrimeReactPTOptions = {
  ...dropdownStyles,
  ...selectButtonStyles,
  ...cardStyles,
  ...inputTextStyles,
  ...dialogStyles,
  ...autoCompleteStyle,
  ...calendarStyles,
  ...inputNumberStyles,
  ...dataTableStyles,
  ...contextMenuStyles,
  ...buttonStyles,
  ...confirmPopupStyles,
  ...paginatorStyles,
};
