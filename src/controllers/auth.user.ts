import User from "../models/user.model"
import { Request, Response } from "express"
import bcrypt from "bcryptjs"
import { AccesToken } from "../utils/generate.token"

export const register = async (req: Request, res: Response) => {
    const { email, password, username } = req.body
    try {
        const passhash = await bcrypt.hash(password, 10)
        const newUser = new User({
            email, password: passhash, username
        })
        const userSave = await newUser.save();
        const token = await AccesToken({ id: userSave._id.toString() })
        res.cookie("token", token)
        console.log(userSave)
        res.json({
            id: userSave._id,
            username: userSave.username,
            email: userSave.email,
            createAT: userSave.createdAt,
            updateAT: userSave.updatedAt
        });

    } catch (error) {
        console.log(error)
        res.status(500).json({
            error: error
        });
    }
}