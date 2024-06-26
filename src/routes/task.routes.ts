import { Router } from "express";
import { deleteTask, getTasks, postTask, getTaskbyId, updateTask } from "../controllers/task.controller";

const taskRouter = Router();

taskRouter.get("/task", getTasks);
taskRouter.get("/task/:index", getTaskbyId)
taskRouter.post("/task", postTask);
taskRouter.put("/task/:index", updateTask)
taskRouter.delete("/task/:index", deleteTask);

export default taskRouter;