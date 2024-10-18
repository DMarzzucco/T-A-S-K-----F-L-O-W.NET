import { Injectable } from "@nestjs/common";
import { PassportStrategy } from "@nestjs/passport";
import { ExtractJwt, Strategy } from "passport-jwt";
import { Request } from "express";
import { PayLoadToken } from "../interface/auth.interface";
import { AuthService } from "../services/auth.service";

@Injectable()
export class JwtRefreshStrategy extends PassportStrategy(Strategy, "jwt-refresh") {
    constructor(private readonly authService: AuthService) {
        super({
            jwtFromRequest: ExtractJwt.fromExtractors([
                (req: Request) => req.cookies?.Refresh
            ]),
            secretOrKey: process.env.REFRESH_TOKEN_KEY,
            passReqToCallback: true
        })
    }
    async validate(req: Request, payload: PayLoadToken) {
        const user = await this.authService.veryfyRefreshToken(req.cookies?.Refresh, payload.sub)
        return user
    }
}