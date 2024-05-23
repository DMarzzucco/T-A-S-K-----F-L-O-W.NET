import express from 'express';
import cors from 'cors';
// ->
const App = express();
const Port = process.env.PORT || 3000;
// ->
App.use(cors());
App.use(express.json());
// ->
let messages: string[] = [];
// ->reitarar los datos impuestos en fetch
App.get('/message', (_req, res) => {
    res.json(messages)
});

App.post('/message', (req, res) => {
    const message = req.body.message;
    try {
        if (typeof message === 'string') {
            messages.push(message);
            res.status(201).send('Message Added')
        } else {
            throw new Error('Invalid Message Format')
        }
    } catch (error) {
        res.status(400).send(error instanceof Error ? error.message : 'Uknown Error');
    }
});
// ->
App.listen(Port, () => {
    console.log(`Server listen at port ${Port}`)
})