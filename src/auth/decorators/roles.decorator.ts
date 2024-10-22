import { SetMetadata } from "@nestjs/common";
import { ROLES_KEY } from "../../constants/keyDecorator.constants";
import { ROLES } from "@prisma/client";

export const Roles = (...roles: Array<keyof typeof ROLES>) => SetMetadata(ROLES_KEY, roles)