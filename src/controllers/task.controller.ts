import { Request, Response } from "express";
import { IMessage } from "../interfaces/IMessage";

let Messages: IMessage[] = [];

export const getTask = async (_req: Request, res: Response) => {
    res.json(Messages);
    console.log("connect")
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
        return res.status(401).send(error instanceof Error ? error.message : 'Uknown Error')
        console.log(error)
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
