import { Router } from "express";
import { authRequired } from "../middlewares/validate";
import { TaskControlls } from "../controllers/task.controller";

const taskRouter = Router();
const TaskContr = new TaskControlls()

taskRouter.get("/task", authRequired, TaskContr.getTasks);
taskRouter.get("/task/:id", authRequired, TaskContr.getTaskbyId)
taskRouter.post("/task", authRequired, TaskContr.postTask);
taskRouter.put("/task/:id", authRequired, TaskContr.updateTask)
taskRouter.delete("/task/:id", authRequired, TaskContr.deleteTask);

taskRouter.get("/allt", TaskContr.getAllTask)
taskRouter.delete("/allt", TaskContr.deleteAll)

export default taskRouter;