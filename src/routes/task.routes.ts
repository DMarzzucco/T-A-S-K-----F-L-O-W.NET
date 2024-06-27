import { Router } from "express";
import { deleteTask, getTasks, postTask, getTaskbyId, updateTask, getAllTask, deleteAll } from "../controllers/task.controller";
import { authRequired } from "../middlewares/validate";

const taskRouter = Router();
taskRouter.get("/task", authRequired, getTasks);
taskRouter.get("/task/:id", authRequired, getTaskbyId)
taskRouter.post("/task", authRequired, postTask);
taskRouter.put("/task/:id", authRequired, updateTask)
taskRouter.delete("/task/:id", authRequired, deleteTask);

taskRouter.get("/allt", getAllTask)
taskRouter.delete("/allt", deleteAll)

export default taskRouter;