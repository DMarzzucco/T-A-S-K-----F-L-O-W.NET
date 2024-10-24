import { Injectable, UnauthorizedException } from '@nestjs/common';
import { JwtService } from '@nestjs/jwt';
import { UsersService } from '../../users/users.service';
import * as bcrypt from "bcrypt"
import { PayLoadToken } from '../interface/auth.interface';
import { User } from '@prisma/client';
import { Request, Response } from 'express';

@Injectable()
export class AuthService {
    constructor(
        private readonly jwtService: JwtService,
        private readonly userService: UsersService,
    ) { }

    async generateToken(user: User, res: Response): Promise<any> {
        const payload: PayLoadToken = { roles: user.roles, sub: user.id }

        const expirationDate = 24 * 60 * 60;
        const RefreshExpirationDate = 7 * 24 * 60 * 60;

        const access_token = this.jwtService.sign(payload, {
            secret: process.env.SECRET_KEY,
            expiresIn: `${expirationDate}`
        })
        const refresh_token = this.jwtService.sign(payload, {
            secret: process.env.REFRESH_TOKEN_KEY,
            expiresIn: `${RefreshExpirationDate}`
        })
        const hashRefresh_token = await bcrypt.hash(refresh_token, 10)
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
    async validationUser(username: string, password: string): Promise<User> {
        const user = await this.userService.finBy({ key: "username", value: username })

        const match = await bcrypt.compare(password, user.password)
        if (!match) throw new UnauthorizedException("Password is Wrong")

        return user
    }
    async veryfyRefreshToken(refreshToken: string, userId: number): Promise<User> {
        const user = await this.userService.findOne(userId)
        if (!user) throw new UnauthorizedException("User not found")

        const authenticated = await bcrypt.compare(refreshToken, user.refreshToken)
        if (!authenticated) throw new UnauthorizedException()

        return user
    }

    async profile(req: Request) {
        const token = req.cookies["Authentication"]
        if (!token) throw new UnauthorizedException("No token found in cookies")

        const decodeToken = this.jwtService.verify(token, { secret: process.env.SECRET_KEY })
        const userid = decodeToken.sub

        const user = await this.userService.findOne(userid)
        return ({ username: user.username })
    }

    async logOut(userId: number, res: Response) {
        await this.userService.updateToken(userId, null)

        res.cookie("Authenticated", "", {
            expires: new Date(0),
            httpOnly: true,
            secure: process.env.NODE_ENV === "production"
        })
        res.cookie("Refresh", "", {
            expires: new Date(0),
            httpOnly: true,
            secure: process.env.NODE_ENV === "production"
        })
    }
}

