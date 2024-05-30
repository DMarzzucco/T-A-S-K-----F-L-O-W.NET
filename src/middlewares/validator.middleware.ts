import { Request, Response, NextFunction } from "express"
import { AnyZodObject } from "zod"

export const validateSchema = (Schema: AnyZodObject) =>
    (req: Request, res: Response, next: NextFunction) => {
        try {
            Schema.parse(req.body);
            next();
        } catch (error) {
            if (error instanceof Error && 'error' in error) {
                res.status(400).json(error)
                return;
            }
            res.status(500).json({ error })
            return;
        }
    }