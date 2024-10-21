import { ExecutionContext, Injectable, } from '@nestjs/common';
import { Reflector } from '@nestjs/core';
import { AuthGuard } from '@nestjs/passport';
import { IS_PUBLIC } from '../../constants/keyDecorator.constants';

@Injectable()
export class JwtAuthGuard extends AuthGuard("jwt") {

  constructor(
    private readonly reflector: Reflector,
  ) { super() }

   canActivate(context: ExecutionContext,) {

    const isPublic = this.reflector.get(IS_PUBLIC, context.getHandler())
    if (isPublic) return true

    return super.canActivate(context)
  }
}
