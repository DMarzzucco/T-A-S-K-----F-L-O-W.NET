import { Router } from "express";
import { login, register } from "../controllers/auth.user";
// import { authRequired } from "../middlewares/validate";
import { validateSchema } from "../middlewares/validator.middleware";
import { loginSchema, registerSchema } from "../models/schemas/auth.schemas";

const userRoute = Router();

userRoute.post('/register', validateSchema(registerSchema), register);
userRoute.post('/login', validateSchema(loginSchema), login);

export default userRoute;