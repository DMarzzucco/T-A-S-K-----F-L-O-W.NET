import User from "../models/user.model"
import { Request, Response } from "express"
import bcrypt from "bcryptjs"
import { AccesToken } from "../utils/generate.token"
import { AuthenticateRequest } from "../interfaces/IMessage"

export const register = async (req: Request, res: Response) => {
    const { email, password, username } = req.body
    try {
        const userFound = await User.findOne({ email });
        if (userFound) {
            res.status(400).json({ errors: [{ message: "The email already exists" }] })
            // res.status(400).json(["The email already exists"])
            return;
        }

        const passhash = await bcrypt.hash(password, 10)
        const newUser = new User({
            email, password: passhash, username
        })
        const userSave = await newUser.save();
        const token = await AccesToken({ id: userSave._id.toString() })
        res.cookie("token", token)
        console.log("User Register ")
        res.json({
            id: userSave._id,
            username: userSave.username,
            email: userSave.email,
            createAT: userSave.createdAt,
            updateAT: userSave.updatedAt
        });

    } catch (error) {
        console.log(error)
        res.status(500).json({ errors: [{ message: "Server error" }] });
    }
}

export const login = async (req: Request, res: Response) => {
    const { username, password } = req.body
    try {
        const UserFound = await User.findOne({ username })
        if (!UserFound) {
            res.status(400).json({ message: "user not found " })
            return;
        }
        if (UserFound.password) {
            const isMatch = await bcrypt.compare(password, UserFound.password);
            if (isMatch) {
                const token = await AccesToken({ id: UserFound._id.toString() })
                res.cookie("token", token)
                res.json({
                    username: UserFound.username,
                    message: "Welcome"
                })
                console.log(UserFound)
            } else {
                console.log("Invalid Password")
                res.status(400).json({
                    message: "Invalid password"
                })
            }
        }

    } catch (error) {
        console.error(error)
        res.status(500).json({ error: error })
    }
}

export const logout = async (_req: Request, res: Response) => {
    res.cookie('token', "", {
        expires: new Date(0)
    })
    res.json({ message: "Session out" })
}

export const profile = async (req: AuthenticateRequest, res: Response) => {
    const userFound = await User.findById(req.user?.id)
    try {
        if (!userFound) {
            res.status(404).json({
                message: "User not found"
            })
            return;
        }
        return res.json({
            if: userFound._id,
            username: userFound.username,
            email: userFound.email,
            message: "Your are in the profile "
        })

    } catch (error) {
        res.status(500).json({
            error: error
        })
        return;
    }
}