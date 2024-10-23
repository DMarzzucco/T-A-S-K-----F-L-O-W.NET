import { Injectable, UnauthorizedException } from "@nestjs/common";
import { PassportStrategy } from "@nestjs/passport";
import { Request } from "express";
import { ExtractJwt, Strategy } from "passport-jwt";
import { PayLoadToken } from "../interface/auth.interface";
import { UsersService } from "../../users/users.service";


@Injectable()
export class JwtStrategy extends PassportStrategy(Strategy, "jwt") {
    constructor(private readonly userService: UsersService) {
        super({
            jwtFromRequest: ExtractJwt.fromExtractors([
                (req: Request) => req.cookies?.Authentication
            ]),
            secretOrKey: process.env.SECRET_KEY,
            ignoreExpiration: true
        })
    }

    async validate(payload: PayLoadToken) {
        if (!payload.sub) throw new UnauthorizedException("Token does not conatin a valid user ID")
        const user = await this.userService.findOne(payload.sub)
        return {
            idUser: user.id,
            roleUser: user.roles
        }
    }
}