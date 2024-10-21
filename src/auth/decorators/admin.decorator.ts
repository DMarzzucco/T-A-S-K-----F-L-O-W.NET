import { SetMetadata } from "@nestjs/common";
import { ADMIN_KEY } from "../../constants/keyDecorator.constants";
import { ROLES } from "../../constants/roles.constants";

export const AdminAcces = () => SetMetadata(ADMIN_KEY, ROLES.ADMIN)