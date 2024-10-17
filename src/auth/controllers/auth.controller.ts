import { Body, Controller, Post, Req, Res, UnauthorizedException, UseGuards } from '@nestjs/common';
import { AuthService } from '../services/auth.service';
import { Request, Response } from 'express';
import { ApiBody, ApiTags } from '@nestjs/swagger';
import { AuthDTO } from '../dto/create-auth.dto';
import { AuthGuard } from '@nestjs/passport';
import { User } from '@prisma/client';
import { CurrentUser } from '../decorators/current-user.decorator';
import { LocalAuthGuard } from '../guards/local-auth.guard';

@ApiTags("Auth")
@Controller('auth')
export class AuthController {
    constructor( private readonly service: AuthService ) { }

    @UseGuards(LocalAuthGuard)
    @ApiBody({ type: AuthDTO })
    @Post()
    async login(
        // @Req() req: Request,
        // @Body() { username, password }: AuthDTO
        @CurrentUser() body: User,
        @Res({ passthrough: true }) res: Response
    ) {
        // const user = await this.service.validationUser(body.username, body.password)
        // if (!user) throw new UnauthorizedException("Incorrect username or  non-existent username")
        return await this.service.generateToken(body, res)

        // const user = await this.service.validationUser(username, password)
        // if (!user) {
        //     throw new UnauthorizedException("Incorrect username or  non-existent username")
        // }
        // const data = req.user as User
        // return this.service.generateToken(data)
    }
}

