import { Router } from "express";
import { VeryToken, deleteUser, login, logout, profile, register } from "../controllers/auth.user";
import { authRequired } from "../middlewares/validate";
import { validateSchema } from "../middlewares/validator.middleware";
import { loginSchema, registerSchema } from "../models/schemas/auth.schemas";

const userRoute = Router();

userRoute.post('/Register', validateSchema(registerSchema), register);
userRoute.post('/Login', validateSchema(loginSchema), login);
userRoute.post('/Logout', logout);
userRoute.get('/very', VeryToken)
userRoute.get('/Profile', authRequired, profile)
userRoute.delete("/delete/:index", authRequired, deleteUser)

export default userRoute;