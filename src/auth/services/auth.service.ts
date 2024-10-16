import { Injectable, NotFoundException, UnauthorizedException } from '@nestjs/common';
import { JwtService } from '@nestjs/jwt';
import { UsersService } from '../../users/users.service';
import * as bcrypt from "bcrypt"
import { PayLoadToken } from '../interface/auth.interface';
import { User } from '@prisma/client';

@Injectable()
export class AuthService {
    constructor(
        private readonly jwtService: JwtService,
        private readonly userService: UsersService,
    ) { }

    async validationUser(username: string, password: string) {
        const user = await this.userService.finBy({ key: "username", value: username })

        if (!user) {
            throw new NotFoundException("user not found")
        }

        const match = await bcrypt.compare(password, user.password)
        if (!match) {
            throw new UnauthorizedException("password is wrong")
        }

        return user
    }

    async generateToken(user: User): Promise<any> {
        const payload: PayLoadToken = { roles: user.roles, sub: user.id }
        const access_token = this.jwtService.sign(payload)
        return {
            // access_token: this.jwtService.sign(payload)
            access_token, user
        }
    }
}

