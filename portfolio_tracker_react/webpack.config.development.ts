import { Configuration } from "webpack";
import { Configuration as DevConfiguration } from "webpack-dev-server";
import HtmlWebpackPlugin from "html-webpack-plugin";
import path from "path";

const config: Configuration | DevConfiguration = {
  mode: "development",
  devtool: "eval-cheap-module-source-map",
  entry: ["./src/index.tsx"],
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: {
          loader: "ts-loader",
          options: {
            transpileOnly: true,
          },
        },
        exclude: /node_modules/,
      },
      {
        test: /\.css$/i,
        use: ["style-loader", "css-loader", "postcss-loader"],
      },
    ],
  },
  devServer: {
    port: 8080,
    historyApiFallback: {
      rewrites: [
        { from: /./, to: "/index.html" }, // Redirect all requests to index.html
      ],
    },
    static: path.resolve(__dirname, "dev-server"),
    hot: true,
    liveReload: false,
    proxy: [
      {
        context: ["/api"],
        target: "http://localhost:5255",
        // target:
        //   process.env.services__portfoliotrackerwebapp__https__0 ||
        //   process.env.services__portfoliotrackerwebapp__http__0,
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
    publicPath: "/",
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
  cache: {
    type: "filesystem",
  },
  watchOptions: {
    ignored: /node_modules/,
  },
};

export default config;
