import { User } from "@prisma/client";
import { CreateUserDto } from "../dto/create-user.dto";
import { findByProps } from "./user.interface";
import { UpdateUserDto } from "../dto/update-user.dto";

export interface UserServiceProps {

    create(body: CreateUserDto): Promise<CreateUserDto>;

    findAll(): Promise<User[]>;

    findOne(id: number): Promise<User>;

    finBy(props: findByProps): Promise<User>;

    update(id: number, data: UpdateUserDto): Promise<UpdateUserDto>

    updateToken(id: number, refreshToken: string): Promise<User>;

    remove(id: number): Promise<User>;
}