import { User } from "@prisma/client";

export interface UserCreate extends Omit<User, "id" | "create_time" | "update_time" | "refreshToken"> { }

export interface findByProps {
    key: keyof User;
    value: any
}