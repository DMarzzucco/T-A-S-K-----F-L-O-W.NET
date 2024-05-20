import { Router } from "express";
import fs from 'fs';
import path from 'path';
import * as express from 'express';

const routMovie =  Router ();

routMovie.use (express.json());
const joinPath = path.join ()