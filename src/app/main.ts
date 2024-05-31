import express from "express";
import cors from "cors";
import cookieParser from "cookie-parser"
import { taskRoutes, authRoutes } from "../routes";

const App = express();

const corsOptions = {
  origin: 'http://localhost:5173',
  credentials: true
};
App.use(cors(corsOptions));
App.use(express.json());
App.use(cookieParser());

App.use("/api", authRoutes);
App.use("/api", taskRoutes);

export default App;
