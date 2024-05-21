import { Router } from "express";
import fetch from "node-fetch";

const user = Router();
// prueba 
user.get('/', async (_req, res) => {
    try {
        const respond = await fetch('https://my-json-server.typicode.com/typicode/demo/posts');
        const useRes = await respond.json();
        res.json(useRes);
    }
    catch (error) {
        res.status(500).json({ error: 'error interno' });
    }
})
export default user;