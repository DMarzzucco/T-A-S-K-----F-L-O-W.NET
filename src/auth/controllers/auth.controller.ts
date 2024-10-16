import { Body, Controller, Post, Req, UnauthorizedException, UseGuards } from '@nestjs/common';
import { AuthService } from '../services/auth.service';
import { Request } from 'express';
import { ApiTags } from '@nestjs/swagger';
import { AuthDTO } from '../dto/create-auth.dto';
import { AuthGuard } from '@nestjs/passport';
import { User } from '@prisma/client';

@ApiTags("Auth")
@Controller('auth')
export class AuthController {
    constructor(
        private readonly service: AuthService
    ) { }

    @UseGuards(AuthGuard("local"))
    @Post()
    async login(@Req() req: Request, @Body() { username, password }: AuthDTO) {
        const user = await this.service.validationUser(username, password)
        if (!user) {
            throw new UnauthorizedException("Incorrect username or  non-existent username")
        }
        const data =  req.user as User
        return this.service.generateToken(data)
    }
}

