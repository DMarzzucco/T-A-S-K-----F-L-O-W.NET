import express from "express";
import morgan from "morgan";
// import router from "./routes/index.js";
import routerMo from "./routes/movies.js";
import prueba from "./routes/prueba.js";
import user from "./routes/user.js";

const app = express();
// app.use(router);
app.use ('/api/movies', routerMo);
app.use(prueba);
app.use ('/api/user',user);


// OPCIONES;
app.set('port', process.env.PORT || 3000);
// const port= process.env.PORT || 3000;  
// en el caso que haya un puerto disponible lo va a tomar 
//  de lo contrario || solo tomar el puerto 3000 
app.set('json spaces', 2);


// MIDDLWARES
// (de esta manera se activa morgan, lo cual
// no s permite verficiar si alguien entro o acutalizo algo
// desde nuestro servidor)
app.use(morgan('dev'));
app.use(express.urlencoded({ extended: false }));
app.use(express.json());

// comienzo del servidor para que funcione
app.listen(app.get('port'), () => {
    // ${app.get('port')} $valor {aplicacion.importar (puerto/3000)}
    console.log(`Server is running on port ${app.get('port')}`);
})


