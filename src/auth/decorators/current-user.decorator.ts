import { createParamDecorator, ExecutionContext } from "@nestjs/common";
import { getCurrentUserbyContext } from "../../constants/getUserbyContext.constants";

export const CurrentUser = createParamDecorator(
    (_data: unknown, context: ExecutionContext) =>
        getCurrentUserbyContext(context)
)