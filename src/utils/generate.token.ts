import { Payload } from "../interfaces/IMessage";
import { ScreetToken } from "./config";
import jwt from "jsonwebtoken";

export function AccesToken(payload: Payload): Promise<string> {
    return new Promise((resolve, reject) => {
        jwt.sign(payload, ScreetToken, (err, token) => {
            if (err) {
                reject(err)
            } else {
                if (token) {
                    resolve(token)
                } else {
                    reject(new Error('TOken generation falled without an error'))
                }
            }
        });
    });
}
