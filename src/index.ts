import express, { Request, Response } from "express";
import cors from "cors";
// -->
interface Message {
    id: number;
    text: string;
}
// -->
const App = express();
const Port = process.env.PORT || 3000;
// -->
App.use(cors());
App.use(express.json());
// -->
let Messages: string[] = [];
// -->
App.get('/message', (_req: Request, res: Response) => { res.json(Messages); })
App.post('/message', (req: Request, res: Response) => {
    const message: Message = req.body.message;
    try {
        if (typeof message === 'string') {
            Messages.push(message);
            res.status(201).send('Message Added');
            console.log(message);
        } else {
            throw new Error('invalid Format');
        }
    } catch (error) {
        res.status(400).send(error instanceof Error ? error.message : 'Uknow Error');
        return;
    }
})
App.delete('/message/:index', (req: Request, res: Response) => {
    const index: number = parseInt(req.params.index, 10);
    try {
        if (isNaN(index) || index < 0 || index >= Messages.length) {
            return res.status(400).send('Message not Found');
        }
        Messages.splice(index, 1);
        console.log("DELETE");
        return res.status(200).send('Message was delete');

    } catch (error) {
        res.status(401).send(error);
        return;
    }
})
// -->
App.listen(Port, () => {
    console.log(`Server listen at port ${Port}`)
})