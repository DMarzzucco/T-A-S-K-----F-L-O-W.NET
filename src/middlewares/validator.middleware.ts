import { Request, Response, NextFunction } from "express"
import { AnyZodObject, ZodError } from "zod"
import { ErrorResponse, ValidationErrorResponse } from "../interfaces/IMessage";

export const validateSchema = (Schema: AnyZodObject) =>
    (req: Request, res: Response<ValidationErrorResponse | ErrorResponse>, next: NextFunction) => {
        try {
            Schema.parse(req.body);
            next();
        } catch (error: any) {
            if (error instanceof ZodError) {
                const validationError = error.errors.map((issue) => ({
                    message: issue.message
                }))
                res.status(400).json({ errors: validationError })
                return;
            }
            else if (error instanceof Error && 'error' in error) {
                res.status(401).json({ error: error.message })
                return;
            }
            res.status(500).json({ errors: [{ message: "Server error" }] })
            return; 
        }
    }