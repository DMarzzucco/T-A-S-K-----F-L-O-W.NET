import { Router } from "express";
import { VeryToken, deleteAllUsers, deleteUser, login, logout, profile, register, showAllUsers } from "../controllers/auth.user";
import { authRequired } from "../middlewares/validate";
import { validateSchema } from "../middlewares/validator.middleware";
import { loginSchema, registerSchema } from "../models/schemas/auth.schemas";

const userRoute = Router();

userRoute.post('/Register', validateSchema(registerSchema), register);
userRoute.post('/Login', validateSchema(loginSchema), login);
userRoute.post('/task', logout);
userRoute.get('/very', authRequired, VeryToken)
userRoute.get('/Profile', authRequired, profile)
userRoute.delete("/delete/:index", authRequired, deleteUser)
// 
userRoute.get('/Users', showAllUsers);
userRoute.delete('/Users', deleteAllUsers)

export default userRoute;