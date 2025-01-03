import { createSlice } from "@reduxjs/toolkit";
import { IStockItem } from "../Api";

interface IAssetsState {
  list: IStockItem[];
}

const initialState: IAssetsState = {
  list: [],
};

export const assetsSlice = createSlice({
  name: "assets",
  initialState,
  reducers: {},
});
