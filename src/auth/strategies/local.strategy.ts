import { Injectable, UnauthorizedException } from "@nestjs/common";
import { PassportStrategy } from "@nestjs/passport";
import { Strategy } from "passport-local";
import { AuthService } from "../services/auth.service";
import { User } from "@prisma/client";

@Injectable()
export class LocalStrategy extends PassportStrategy(Strategy, "local") {
    constructor(
        private readonly authService: AuthService
    ) {
        super({
            usernameField: "username",
            passwordField: "password"
        })
    }

    async validate(username: string, password: string) {
        console.log('Credenciales recibidas:', { username, password });
        
        const user: User = await this.authService.validationUser(username, password)
        if (!user) throw new UnauthorizedException("Not Allowed")

        return user
    }
}