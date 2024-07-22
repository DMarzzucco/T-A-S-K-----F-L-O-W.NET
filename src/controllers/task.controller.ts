import { Response } from "express";
import { AuthenticateRequest } from "../interfaces/IMessage";
import Task from "../models/task.models";

export class TaskControlls{
    
    public getTasks = async (req: AuthenticateRequest, res: Response) => {
        const tasks = await Task.find({ user: req.user?.id }).populate('user')
        try {
            if (!tasks || tasks.length === 0) {
                res.status(400).json({
                    errors: [{
                        message: "Not exist tasks yet "
                    }]
                })
                return;
            }
            return res.json({ tasks })
        } catch (error) {
            res.status(500).json({ error: [{ message: "Server error" }] })
            return;
        }
    }
    
    public getTaskbyId = async (req: AuthenticateRequest, res: Response) => {
        const taskFound = await Task.findById(req.params.id).populate('user')
        try {
            if (!taskFound) {
                res.status(400).json({ errors: [{ message: "task not found" }] })
                return
            }
            return res.json({
                id: taskFound._id,
                title: taskFound.title,
                description: taskFound.description,
                user: taskFound.user,
                message: "Task found it"
            })
        } catch (error) {
            res.status(500).json({ error: [{ message: "server error" }] })
            return;
        }
    }
    
    public postTask = async (req: AuthenticateRequest, res: Response) => {
        const { title, description, data } = req.body;
        try {
            const newTask = new Task({ title, description, data, user: req.user?.id });
            const saveTasks = await newTask.save()
            return res.json({
                id: saveTasks._id,
                title: saveTasks.title,
                description: saveTasks.title,
                user: saveTasks.user
    
            });
        } catch (error) {
            console.error(error)
            res.status(500).json({ error: [{ message: "Server error " }] })
            return;
        }
    }
    
    public updateTask = async (req: AuthenticateRequest, res: Response) => {
        const Taskupdate = await Task.findByIdAndUpdate(req.params.id, req.body, { new: true })
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
    
    public deleteTask = async (req: AuthenticateRequest, res: Response) => {
        const Taskdelete = await Task.findByIdAndDelete(req.params.id)
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
    // 
    public getAllTask = async (_req: AuthenticateRequest, res: Response) => {
        const task = await Task.find()
        try {
            if (!task) {
                res.status(400).json({ errors: [{ message: "Task not found  " }] })
                return;
            }
            if (task.length === 0) {
                res.status(200).json({ message: [{ message: "No one task yet" }] })
                return;
            }
            return res.json(task)
        } catch (error) {
            res.status(500).json({ error: [{ message: "server error" }] })
            return;
        }
    }
    
    public deleteAll = async (_req: AuthenticateRequest, res: Response) => {
        const task = await Task.deleteMany()
        try {
            if (!task) {
                res.status(400).json({ errors: [{ mesage: "tasks not found it" }] })
                return
            }
            return res.status(200).json({ message: "All task was delete it " })
        } catch (error) {
            return res.status(500).json({ error: [{ mesage: "server error" }] })
        }
    }
    
}


