import { Router } from "express";
import { deleteTask, getTask, postTask } from "../controllers/task.controller";

const router = Router();

router.get("/message", getTask);
router.post("/message", postTask);
router.delete("/message/:index", deleteTask);

export default router;