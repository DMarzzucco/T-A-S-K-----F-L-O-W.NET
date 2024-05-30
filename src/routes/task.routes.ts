import { Router } from "express";
import { deleteTask, getTask, postTask } from "../controllers/task.controller";

const taskRouter = Router();

taskRouter.get("/message", getTask);
taskRouter.post("/message", postTask);
taskRouter.delete("/message/:index", deleteTask);

export default taskRouter;