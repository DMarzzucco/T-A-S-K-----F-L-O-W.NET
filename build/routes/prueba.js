import { Router } from "express";
import fs from 'fs';
import path from 'path';
const prueba = Router();
prueba.get('/prueba', (_req, res) => {
    const jsDocument = path.join(__dirname, '../seconSample.json');
    fs.readFile(jsDocument, 'utf-8', (error, data) => {
        if (error) {
            res.status(500).send("no se puede leer");
            return;
        }
        try {
            const movie = JSON.parse(data);
            res.json(movie);
        }
        catch (error) {
            res.status(500).send(error);
            return;
        }
    });
});
export default prueba;
