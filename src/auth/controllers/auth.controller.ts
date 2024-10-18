import {  Controller, Post, Res, UnauthorizedException, UseGuards } from '@nestjs/common';
import { AuthService } from '../services/auth.service';
import { Response } from 'express';
import { ApiBody, ApiTags } from '@nestjs/swagger';
import { AuthDTO } from '../dto/create-auth.dto';
import { User } from '@prisma/client';
import { CurrentUser } from '../decorators/current-user.decorator';
import { LocalAuthGuard } from '../guards/local-auth.guard';
import { JwtRefreshAuthGuard } from '../guards/jwt-refresh-auth.guard';

@ApiTags("Auth")
@Controller('auth')
export class AuthController {
    constructor( private readonly service: AuthService ) { }

    @UseGuards(LocalAuthGuard)
    @ApiBody({ type: AuthDTO })
    @Post()
    async login(
        @CurrentUser() body: User,
        @Res({ passthrough: true }) res: Response
    ) {
        return await this.service.generateToken(body, res)
    }

    @UseGuards(JwtRefreshAuthGuard)
    @ApiBody({ type: AuthDTO })
    @Post("refresh")
    async refreshToken (
        @CurrentUser() body: User,
        @Res({ passthrough: true }) res: Response
    ){
        return await this.service.generateToken(body, res)
    }
}

