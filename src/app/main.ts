import express from "express";
import cors from "cors";
import taskRouter from "../routes/task.routes";

const App = express();

const corsOptions ={
  origin:'http://localhost:5173',
  credentials:true
};
App.use(cors(corsOptions));
App.use(express.json());

App.use("/api", taskRouter);

export default App;
