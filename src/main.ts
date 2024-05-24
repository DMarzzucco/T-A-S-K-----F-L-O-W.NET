import express from "express";
import cors from "cors";
import router from "./routes/messageRoutes";

const App = express();

App.use(cors());
App.use(express.json());

App.use(router);

export default App;
