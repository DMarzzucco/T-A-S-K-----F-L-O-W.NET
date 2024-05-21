import express from "express";
import user from "./routes/user.ts";
import prueba from "./routes/prueba.ts";
const app = express();
app.use('/user', user);
app.use(prueba);
app.use(express.json());
const port = process.env.PORT || 3000;
app.get('/', (_req, res) => {
    console.log('conectado');
    const data = {
        "name": "juan",
        "lastname": "prachek"
    };
    res.json(data);
});
app.listen(port, () => {
    console.log(`servidor conectado en el puerto ${port}`);
});
