import jwt, { VerifyErrors } from "jsonwebtoken";
import { Response, NextFunction } from "express";
import { ScreetToken } from "../utils/config";
import { AuthenticateRequest, userPayload } from "../interfaces/IMessage";

export const authRequired = (req: AuthenticateRequest, res: Response, next: NextFunction): void => {
    const token = req.cookies['token'];
    if (!token) {
        res.status(400).json({ message: 'you are note authorized' })
        return;
    }
    jwt.verify(token, ScreetToken, (err: VerifyErrors | null, decoded: any) => {
        if (err) {
            res.status(401).json({ message: "token fail" })
            return;
        }
        if (decoded) {
            req.user = decoded as userPayload;
            next();
        } else {
            res.status(400).json({ message: "token fail" });
        }
    })
}