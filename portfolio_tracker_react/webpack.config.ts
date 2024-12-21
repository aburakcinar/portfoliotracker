import { Configuration } from "webpack";
import { Configuration as DevConfiguration } from "webpack-dev-server";
import HtmlWebpackPlugin from "html-webpack-plugin";
import path from "path";

const config: Configuration | DevConfiguration = {
  mode: "production",
  devtool: "source-map",
  entry: ["./src/index.tsx"],
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.css$/i,
        use: ["style-loader", "css-loader", "postcss-loader"],
      },
    ],
  },
  devServer: {
    historyApiFallback: true,
    static: path.resolve(__dirname, "dev-server"),
    hot: true,
    proxy: [
      {
        context: ["/api"],
        target: "http://localhost:5255",
      },
    ],
    watchFiles: ["src/**/*"],
  },
  resolve: {
    extensions: [".ts", ".tsx", ".js"],
  },
  output: {
    path: path.resolve(__dirname, "Assets"),
    filename: "bundle.js",
    clean: true,
  },
  plugins: [
    new HtmlWebpackPlugin({
      filename: "index.html",
      inject: true,
      template: path.resolve(__dirname, "public", "index.html"),
      minify: {
        removeComments: true,
        collapseWhitespace: false,
      },
    }),
  ],
  performance: {
    maxEntrypointSize: 2 * 1024 * 1024,
    maxAssetSize: 2 * 1024 * 1024,
  },
};

export default config;
