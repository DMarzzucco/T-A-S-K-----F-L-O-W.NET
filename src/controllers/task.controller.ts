import { Request, Response } from "express";
import { AuthenticateRequest } from "../interfaces/IMessage";
import Task from "../models/task.models";
import User from "../models/user.model"

export const getTasks = async (req: AuthenticateRequest, res: Response) => {
    const UserID = await User.findById(req.user?.id)
    const tasks = await Task.find({ user: UserID }).populate('user')
    try {
        if (!tasks) {
            res.status(400).json({ errors: [{ message: "Task not found " }] })
            return;
        }
        res.json({ tasks })
    } catch (error) {
        res.status(500).json({ error: [{ message: "Server error" }] })
        return;
    }
}

export const getTaskbyId = async (req: AuthenticateRequest, res: Response) => {
    const taskFound = await Task.findById(req.task?.id)
    try {
        if (!taskFound) {
            res.status(400).json({ errors: [{ message: "task not found" }] })
            return
        }
        return res.json({
            id: taskFound._id,
            title: taskFound.title,
            description: taskFound.description,
            message: "Task found it"
        })
    } catch (error) {
        res.status(500).json({ error: [{ message: "server error" }] })
        return;
    }
}

export const postTask = async (req: Request, res: Response) => {
    const { title, description, data } = req.body;
    try {
        const newTask = new Task({ title, description, data });
        const saveTasks = await newTask.save()
        res.json({ saveTasks });
        return
    } catch (error) {
        console.error(error)
        res.status(500).json({ error: [{ message: "Server error " }] })
    }
}

export const updateTask = async (req: AuthenticateRequest, res: Response) => {
    const taskID = await Task.findById(req.task?.id, req.body, { new: true })
    const Taskupdate = await Task.findByIdAndUpdate(taskID)
    try {
        if (!Taskupdate) {
            res.status(400).json({ errors: [{ message: "Task not found" }] })
            return
        }
        return res.status(200).json({ message: [{ message: "Task was update" }] })
    } catch (error) {
        res.status(500).json({ error: [{ message: "server error" }] })
        return;
    }
}

export const deleteTask = async (req: AuthenticateRequest, res: Response) => {
    const taskID = await Task.findById(req.task?.id)
    const Taskdelete = await Task.findByIdAndDelete(taskID)
    try {
        if (!Taskdelete) {
            res.status(400).json({ errors: [{ message: "taks not found" }] })
            return
        }
        return res.status(200).json({ message: [{ message: "Task was deleted" }] })
    } catch (error) {
        return res.status(500).json({ error: [{ message: "Server Error" }] });
    }
}


