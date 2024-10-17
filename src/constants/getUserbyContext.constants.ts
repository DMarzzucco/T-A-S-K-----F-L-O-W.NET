import { ExecutionContext } from "@nestjs/common";

export const getCurrentUserbyContext = (context: ExecutionContext) =>
    context.switchToHttp().getRequest().user;