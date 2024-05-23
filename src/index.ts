import express from "express";
import cors from 'cors';

const App = express();
const Port = process.env.PORT || 3000;

App.use(cors());
App.use(express.json());

let messages: string[] = [];

App.get('/message', (_req, res) => {
    res.json(messages)
});

App.post('/message', async (req, res) => {
    const message = req.body.message;
    try {
        if (typeof message === 'string') {
            messages.push(message);
            res.status(201).send('Message Aded')
        } else {
            throw new Error('Invalid Message Format')
        }
    } catch (error) {
        res.status(400).send('Invalid Message Format')
    }
});

App.listen(Port, () => {
    console.log(`Server listen at port ${Port}`)
})

