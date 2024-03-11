import { Router } from "express";
import fetch  from "node-fetch";

const user = Router ();



user.get ('/', async  (req, res)=>{
    const respond = await fetch ('https://my-json-server.typicode.com/typicode/demo/posts');
    const useres= await respond.json ();
    res.json (useres);
})

export default user;