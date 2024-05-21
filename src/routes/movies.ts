import { Router } from "express";
import fs from 'fs';
import path from 'path';
import * as express from 'express';

const routMovie = Router();

routMovie.use(express.json());
const jsonPath = path.join(new URL('../sample.json', import.meta.url).pathname)

// res get
routMovie.get('/', (_req, res) => {
    fs.readFile(jsonPath, 'utf8', (error, data) => {
        if (error) {
            console.error(error);
            res.status(500).send('cannot read the file');
            return;
        }
        try {
            const movie = JSON.parse(data);
            res.json(movie);
        } catch (error) {
            console.error(error);
            res.status(500).send(error);
        }
    })
})
// res post
routMovie.post('/', (req, res) => {
    const { title, author, price } = req.body;
    if (title && author && price) {
        fs.readFile(jsonPath, 'utf-8', (error, data) => {
            if (error) {
                console.log(error);
                res.status(500).send('cannot read the file')
                return;
            }
            try {
                const movie = JSON.parse(data);
                const id = movie.length + 1;
                const newMovie = { id, ...req.body };
                console.log(newMovie);

                movie.push(newMovie);
                fs.writeFile(jsonPath, JSON.stringify(movie, null, 2), (error) => {
                    if (error) {
                        console.log(error);
                        res.status(500).send('cannot write the file')
                        return;
                    }
                    // res.json(newMovie)
                    res.send('file saved')
                });
            } catch (error) {
                console.error(error);
                res.status(500).send(error);
            }
        })
    } else {
        res.status(400).send('cannot authentic alles elements');
    }
})

routMovie.put('/:id', (req, res) => {
    const id = parseInt(req.params.id);
    if (isNaN(id)) {
        res.status(400).send('the id not is a number');
    }
})