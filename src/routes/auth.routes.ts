import { Router } from "express";
import { login, logout, profile, register } from "../controllers/auth.user";
import { authRequired } from "../middlewares/validate";
import { validateSchema } from "../middlewares/validator.middleware";
import { loginSchema, registerSchema } from "../models/schemas/auth.schemas";

const userRoute = Router();

userRoute.post('/Register', validateSchema(registerSchema), register);
userRoute.post('/login', validateSchema(loginSchema), login);
userRoute.post('/logout', logout);
userRoute.get('/profile', authRequired, profile)

export default userRoute;