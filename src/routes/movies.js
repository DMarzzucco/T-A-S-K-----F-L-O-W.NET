import { Router } from "express";
import fs from 'fs';
import path from "path";

const routerMo = Router();

routerMo.get('/', (req, res) => {
    const jsonPath = path.join(new URL('C:/Users/darma/OneDrive/Documentos/GitHub/Backend/src/sample.json', import.meta.url).pathname);
    fs.readFile(jsonPath, 'utf8', (err, data) => {
        if (err) {
            console.error(err);
            res.satus(500).send('cannot read the file ');
            return;
        }
        try {
            const movies = JSON.parse(data);
            res.json(movies);
        } catch (error) {
            console.error(error);
            res.status(500).send(error);
        }
    })
})

export default routerMo;