import { Router, Request, Response } from "express";
import { IMessage } from "../interfaces/IMessage";

const router = Router();
let Messages: IMessage[] = [];

router.get('/messages', (_req: Request, res: Response) => {
    res.json(Messages);
})
router.post('/message', (req: Request, res: Response) => {
    const message: IMessage = req.body.message;
    try {
        if (typeof message === 'string') {
            Messages.push(message);
            res.status(201).send('Message Added');
            console.log(message);
        } else {
            throw new Error('Invalid Format');
        }
    } catch (error) {
        res.status(401).send(error instanceof Error ? error.message : 'Uknown Error')
        return;
    }
})
router.delete('/message/:index', (req: Request, res: Response) => {
    const index: number = parseInt(req.params.index, 10);
    try {
        if (isNaN(index) || index < 0 || index >= Messages.length) {
            return res.status(400).send('Message not Found');
        }
        Messages.splice(index, 1);
        console.log('DELETE');
        return res.status(200).send('Message was deleted');
    } catch (error) {
        res.status(401).send(error);
        return;
    }
})

export default router;