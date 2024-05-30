import { Request, Response } from "express";
import { IMessage, AuthenticateRequest, TaskDB } from "../interfaces/IMessage";
import taskModels from "../models/task.models";

let Messages: IMessage[] = [];

export const getTask = async (req: AuthenticateRequest, res: Response) => {
    try {
        if (!req.user) {
            res.status(401).json({ message: "user not found" })
        }
        const tasks: TaskDB[] = await taskModels.find({ user: req.user?.id }).populate('user');

        const response = {
            tasks,
            Messages: Messages
        }
        // const mongooseTasks = await Task.find({ user: req.user.id }).populate('user');
        // const tasks: TaskDB[] = mongooseTasks.map(task => ({
        //     ...task.toObject(),
        //     title: task.title ?? undefined, 
        // }));
        res.json(response);

    } catch (error) {
        res.status(500).json({ message: error })
    }
}
export const postTask = async (req: Request, res: Response) => {
    const message: IMessage = req.body.message;
    try {
        if (typeof message === 'string') {
            Messages.push(message);
            console.log(message)
            return res.status(201).send('Message Added');
        } else {
            throw new Error('Invalid Format');
        }
    } catch (error) {
        console.log(error)
        return res.status(401).send(error instanceof Error ? error.message : 'Uknown Error')
    }
}

export const deleteTask = async (req: Request, res: Response) => {
    const index: number = parseInt(req.params.index, 10);
    try {
        if (isNaN(index) || index < 0 || index >= Messages.length) {
            return res.status(400).send('Message not was found');
        }
        Messages.splice(index, 1);
        console.log('DELETE');
        return res.status(200).send('Message was deleted');
    } catch (error) {
        return res.status(401).send(error);
    }
}
