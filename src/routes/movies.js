import { Router } from "express";
import fs from 'fs';
import path from "path";
import * as express from 'express';

const routerMo = Router();

routerMo.use(express.json());

const jsonPath = path.join(new URL('C:/Users/darma/OneDrive/Documentos/GitHub/Backend/src/sample.json', import.meta.url).pathname);

// solisitud GET
routerMo.get('/', (req, res) => {
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

// Solicitud POST
routerMo.post('/', (req, res) => {
    // console.log (req.body);
    const { title, author, price } = req.body;
    if (title && author && price) {
        fs.readFile(jsonPath, 'utf8', (error, data) => {
            if (error) {
                console.log(error);
                res.status(500).send('cannot read the file');
                return;
            }
            try {
                const movies = JSON.parse(data);
                const id = movies.length + 1;
                const newMovies = { id, ...req.body };

                console.log(newMovies);

                movies.push(newMovies);
                fs.writeFile(jsonPath, JSON.stringify(movies, null, 2), (error) => {
                    if (error) {
                        console.log(error);
                        res.status(500).send('cannot write the files')
                        return;
                    }
                    // res.json(newMovies);
                    res.send("se a guardado");
                });
            } catch (error) {
                console.error(error);
                res.status(500).send(error);
            }
        })
    } else {
        res.status(400).send('no se autentificaon todos los elementos');
    }
})


// Solicitud PUT
routerMo.put('/:id', (req, res) => {
    const id = parseInt(req.params.id);
    if (isNaN(id)) {
        res.status(400).send('el id no es un numero');
        return;
    }
    const { title, author, price } = req.body;
    if (!title || !author || !price) {
        res.status(400).send('no se autentifacron todos los componentes ')
        return;
    }
    fs.readFile(jsonPath, 'utf8', (error, data) => {
        if (error) {
            console.error(error);
            res.status(500).send('cannot read the file ');
            return;
        }
        try {
            let movies = JSON.parse(data);
            const idToUpdate = movies.findIndex(movies => movies.id === id);
            if (idToUpdate === -1) {
                res.status(404).send('no se encontro el id');
                return;
            }
            movies[idToUpdate] = { id, ...req.body };

            fs.writeFile(jsonPath, JSON.stringify(movies, null, 2), (error) => {
                if (error) {
                    console.error(error);
                    res.status(500).send('cannot write the file ');
                    return;
                }
                res.send('se a actualizado');
            })
        } catch (error) {
            console.error(error);
            res.status(500).send(error);
        }
    });

});



// Solicitud DELETE 
routerMo.delete('/:id', (req, res) => {
    const id = parseInt(req.params.id);
    fs.readFile(jsonPath, 'utf8', (error, data) => {
        if (error) {
            console.error(error);
            res.status(500).send('cannot read the file ');
            return;
        }
        try {
            let movies = JSON.parse(data);
            movies = movies.filter(movies => movies.id !== id);
            fs.writeFile(jsonPath, JSON.stringify(movies, null, 2), (error) => {
                if (error) {
                    console.error(error);
                    res.status(500).send('cannot write the file ');
                    return;
                }
                res.send('se a eliminado');
            });
        } catch (error) {
            console.error(error);
            res.status(500).send(error);
        }
    })
});

export default routerMo;