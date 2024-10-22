import { CanActivate, ExecutionContext, Injectable, UnauthorizedException, } from '@nestjs/common';
import { Reflector } from '@nestjs/core';
import { AuthGuard } from '@nestjs/passport';
import { IS_PUBLIC } from '../../constants/keyDecorator.constants';

@Injectable()
export class JwtAuthGuard extends AuthGuard("jwt") implements CanActivate {

  constructor(
    private readonly reflector: Reflector,
  ) { super() }

  async canActivate(context: ExecutionContext,): Promise<boolean> {

    const isPublic = this.reflector.get(IS_PUBLIC, context.getHandler())
    if (isPublic) return true

    const req = context.switchToHttp().getRequest()
    const token = req.cookies?.Authentication
    
    if (!token) {
      throw new UnauthorizedException("No token Provided")
    }

    try {
      const result = await super.canActivate(context);
      return result as boolean;

    } catch (err) {
      throw new UnauthorizedException("Token is invalid ")
    }
  }
}
