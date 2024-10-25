import { Controller, Get, Post, Req, Res, UnauthorizedException, UseGuards } from '@nestjs/common';
import { AuthService } from '../services/auth.service';
import { Request, Response } from 'express';
import { ApiBearerAuth, ApiBody, ApiTags } from '@nestjs/swagger';
import { AuthDTO } from '../dto/create-auth.dto';
import { User } from '@prisma/client';
import { CurrentUser } from '../decorators/current-user.decorator';
import { LocalAuthGuard } from '../guards/local-auth.guard';
import { JwtRefreshAuthGuard } from '../guards/jwt-refresh-auth.guard';
import { JwtAuthGuard } from '../guards/jwt-auth.guard';

@Controller('auth')
export class AuthController {
    constructor(
        private readonly service: AuthService,) { }

    @ApiTags("Auth")
    @UseGuards(LocalAuthGuard)
    @ApiBody({ type: AuthDTO })
    @Post()
    async login(
        @CurrentUser() body: User,
        @Res({ passthrough: true }) res: Response
    ) {
        return await this.service.generateToken(body, res)
    }

    @ApiTags("Auth")
    @UseGuards(JwtRefreshAuthGuard)
    @ApiBody({ type: AuthDTO })
    @Post("refresh")
    async refreshToken(
        @CurrentUser() body: User,
        @Res({ passthrough: true }) res: Response
    ) {
        return await this.service.generateToken(body, res)
    }

    @ApiTags("Profile")
    @Get("profile")
    @ApiBearerAuth()
    @UseGuards(JwtAuthGuard)
    async Profile(@Req() req: any) {
        return this.service.profile(req)
    }

    @ApiTags("LogOut")
    @Post("logout")
    @ApiBearerAuth()
    @UseGuards(JwtAuthGuard)
    async logout(@Req() req: any, @Res() res: any) {
        const userId = req.user.idUser;
        await this.service.logOut(userId, res)
        return res.status(200).json({ message: "Lgout successful" })
    }
}

