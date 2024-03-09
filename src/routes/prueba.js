import { Router } from "express";
import fs from 'fs';
import path from 'path';

const prueba = Router ();

prueba.get('/prueba', (req,res)=>{
    const jsDucument = path.join (
        new URL('C:/Users/darma/OneDrive/Documentos/GitHub/Backend/src/seconSample.json',
        import.meta.url).pathname);
        fs.readFile (jsDucument, 'utf-8', (error, data)=>{
            if (error){
                res.status(500).send("we can't read the file");
                return;
            }
            try{
                const movies =JSON.parse(data);
                res.json(movies);
            }
            catch (error){
                res.status(500).send(error);
                return;
            }
        })
})
export default prueba;