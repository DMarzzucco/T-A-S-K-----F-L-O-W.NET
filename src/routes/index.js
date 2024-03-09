import { Router } from "express";
const router = Router();

// routes /rutas
router.get('/', (req, res) => {
    const data = {
        "name": "Juan",
        "lastname": "Peresdz"
    }
    res.json(data);
})

export default router;
