import { CanActivate, ExecutionContext, Injectable, UnauthorizedException } from '@nestjs/common';
import { Reflector } from '@nestjs/core';
import { ADMIN_KEY, IS_PUBLIC, ROLES_KEY } from '../../constants/keyDecorator.constants';
import { Request } from 'express';
import { ROLES } from '@prisma/client';

@Injectable()
export class RolesGuard implements CanActivate {
  constructor(
    private readonly reflector: Reflector
  ) {
  }
  canActivate(
    context: ExecutionContext,
  ): boolean | Promise<boolean> {

    const isPublic = this.reflector.get<boolean>(IS_PUBLIC, context.getHandler())
    if (isPublic) return true

    const roles = this.reflector.get<Array<keyof typeof ROLES>>(ROLES_KEY, context.getHandler())
    const admin = this.reflector.get<string>(ADMIN_KEY, context.getHandler())
    const req = context.switchToHttp().getRequest<Request>()

    const {roleUser} = req
    if (roleUser === ROLES.ADMIN) return true

    if (!roles) {
      if (!admin) {
        return true
      } else if (admin && roleUser === admin) {
        return true
      }
      throw new UnauthorizedException(" you are not get acces")
    }

    const isAuth = roles.some((role) => roleUser === role)
    if (!isAuth) {
      console.log ("roles que espera",roles)
      console.log("rol del usuario",roleUser)
      throw new UnauthorizedException("You are not Admin authorized")
    }
    return true;
  }
}
