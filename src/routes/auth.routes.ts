import { Router } from "express";
import { register } from "../controllers/auth.user";
// import { authRequired } from "../middlewares/validate";
import { validateSchema } from "../middlewares/validator.middleware";
import { registerSchema } from "../models/schemas/auth.schemas";

const userRoute = Router();

userRoute.post('/register', validateSchema(registerSchema), register);

export default userRoute;