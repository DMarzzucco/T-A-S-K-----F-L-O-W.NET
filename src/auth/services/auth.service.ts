import { Injectable, NotFoundException, UnauthorizedException } from '@nestjs/common';
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

    async validationUser(username: string, password: string) {
        const user = await this.userService.finBy({ key: "username", value: username })

        const match = await bcrypt.compare(password, user.password)
        if (!match) throw new UnauthorizedException("Password is Wrong")

        return user
    }

    async generateToken(user: User, res: Response): Promise<any> {
        const payload: PayLoadToken = { roles: user.roles, sub: user.id }

        const expirationDate = new Date(Date.now() + 60 * 60 * 1000)

        const access_token = this.jwtService.sign(payload, {
            secret: process.env.SEECRET_KE,
            expiresIn: `${expirationDate}`
        })

        // const refresh_token = this.jwtService.sign(payload, {
        //     secret: process.env.REFRESH_TOKEN_KEY,
        //     expiresIn: `${expirationDate}`
        // })


        res.cookie("Authentication", access_token, {
            httpOnly: true,
            secure: process.env.NODE_ENV === "production",
            expires: expirationDate

        })

        return { access_token, user }
    }
}

