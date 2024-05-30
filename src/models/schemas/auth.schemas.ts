import { z } from "zod";

export const registerSchema = z.object({

    username: z.string({ required_error: "User name required" }),
    email: z.string({ required_error: "Required Email" }).email({}),
    password: z.string({ required_error: "Password required" }).min(6, {
        message: "the password must have 6 carac min"
    })
})

export const loginSchema = z.object({
    email: z.string({ required_error: "Required Email" }).email({}),
    password: z.string({ required_error: "Required Password" }).min(6, {
        message: "the password must have 6 carac min"
    })
})