import { Injectable } from "@nestjs/common";
import { PassportStrategy } from "@nestjs/passport";
import { Request } from "express";
import { ExtractJwt, Strategy } from "passport-jwt";
import { PayLoadToken } from "../interface/auth.interface";
import { UsersService } from "../../users/users.service";


@Injectable()
export class JwtStrategy extends PassportStrategy(Strategy, "jwt") {
    constructor(private readonly userService: UsersService) {
        super({
            // jwtFromRequest: ExtractJwt.fromExtractors([
            //     (req: Request) => req.cookies?.Authentication
            // ]),
            jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
            secretOrKey: process.env.SEECRET_KEY,
            ignoreExpiration: false
        })
    }

    async validate(payload: PayLoadToken) {
        console.log('Payload received:', payload); // Agregar log aquí
        const user =  this.userService.finBy({ key: "id", value: payload.sub });
        console.log('User found:', user)
        return {...user, roles: payload.roles}
    }
}