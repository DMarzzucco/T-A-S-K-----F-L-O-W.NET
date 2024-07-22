import { Router } from "express";
import { authRequired } from "../middlewares/validate";
import { validateSchema } from "../middlewares/validator.middleware";
import { loginSchema, registerSchema } from "../models/schemas/auth.schemas";
import { userControlls } from "../controllers/auth.user";

const userRoute = Router();
const userContr = new userControlls()

userRoute.post('/Register', validateSchema(registerSchema), userContr.register);
userRoute.post('/Login', validateSchema(loginSchema), userContr.login);
userRoute.post('/taskOut', userContr.logout);
userRoute.get('/very', authRequired, userContr.VeryToken)
userRoute.get('/profile/:id', authRequired, userContr.profile)
userRoute.put("/update/:id", authRequired, userContr.putUser);
userRoute.delete("/profile/:id", authRequired, userContr.deleteUser)
// 
userRoute.get('/Users', userContr.showAllUsers);
userRoute.delete('/Users', userContr.deleteAllUsers)

export default userRoute;