import { z } from "zod";

export const registerSchema = z.object({

    username: z.string({ required_error: "User name required" }),
    email: z.string({ required_error: "Required Email" }).email({ message: "The email is invalid" }),
    password: z.string({ required_error: "Password required" }).min(6, {
        message: "The password must be at least 6 characters"
    })
})

export const loginSchema = z.object({
    username: z.string({ required_error: "Username requerid" }),
    password: z.string({ required_error: "Required Password" }).min(6, {
        message: "The password must be at least 6 characters"
    })
})