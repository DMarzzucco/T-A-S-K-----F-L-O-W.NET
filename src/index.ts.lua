

-- 
import express from "express";
import cors from 'cors';
import user from "./routes/user.js";
import prueba from "./routes/prueba.js";
import routMovie from "./routes/movies.js";

const app = express();

app.use(cors());
app.use('/user', user);
app.use(prueba);
app.use(routMovie);


app.use(express.json());

const port = process.env.PORT || 3000;
app.get('/', (_req, res) => {
    console.log('conectado');
    const data = {
        "name": "juan",
        "lastname": "prachek"
    }
    res.json(data);
})

app.listen(port, () => {
    console.log(`servidor conectado en el puerto ${port}`)
})