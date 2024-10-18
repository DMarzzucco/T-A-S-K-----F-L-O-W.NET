import { Injectable, UnauthorizedException } from '@nestjs/common';
import { JwtService } from '@nestjs/jwt';
import { UsersService } from '../../users/users.service';
import * as bcrypt from "bcrypt"
import { PayLoadToken } from '../interface/auth.interface';
import { User } from '@prisma/client';
import { Response } from 'express';

@Injectable()
export class AuthService {
    constructor(
        private readonly jwtService: JwtService,
        private readonly userService: UsersService,
    ) { }

    async generateToken(user: User, res: Response): Promise<any> {
        const payload: PayLoadToken = { roles: user.roles, sub: user.id }

        const expirationDate = 60 * 60;
        const RefreshExpirationDate = 7 * 24 * 60 * 60;

        const access_token = this.jwtService.sign(payload, {
            secret: process.env.SEECRET_KEY,
            expiresIn: `${expirationDate}`
        })
        const refresh_token = this.jwtService.sign(payload, {
            secret: process.env.REFRESH_TOKEN_KEY,
            expiresIn: `${RefreshExpirationDate}`
        })
        const hashRefresh_token = await bcrypt.hash(refresh_token, 10)
        console.log ("user id:", user.id)
        await this.userService.updateToken(user.id, hashRefresh_token)

        res.cookie("Authentication", access_token, {
            httpOnly: true,
            secure: process.env.NODE_ENV === "production",
            expires: new Date(Date.now() + expirationDate * 1000)
        })
        res.cookie("Refresh", refresh_token, {
            httpOnly: true,
            secure: process.env.NODE_ENV === "production",
            expires: new Date(Date.now() + RefreshExpirationDate * 1000)
        })
        return { access_token, user }
    }
    async validationUser(username: string, password: string) {
        const user = await this.userService.finBy({ key: "username", value: username })

        const match = await bcrypt.compare(password, user.password)
        if (!match) throw new UnauthorizedException("Password is Wrong")

        return user
    }
    async veryfyRefreshToken(refreshToken: string, userId: number) {
        const user = await this.userService.findOne(userId)
        if (!user) throw new UnauthorizedException("User not found")

        const authenticated = await bcrypt.compare(refreshToken, user.refreshToken)
        if (!authenticated) throw new UnauthorizedException()

        return user
    }
}

