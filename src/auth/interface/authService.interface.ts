import { User } from "@prisma/client";
import { Request, Response } from "express";

export interface AuthServiceProps {

    generateToken(user: User, res: Response): Promise<any>

    validationUser(username: string, password: string): Promise<User>

    veryfyRefreshToken(refreshToken: string, userId: number): Promise<User>;

    profile(req: Request): Promise<{ username: string }>

    logOut(userId: number, res: Response): Promise<any>
}