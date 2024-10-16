import { $Enums } from "@prisma/client";
// import { ROLES } from "src/constants/roles.constants";

export interface PayLoadToken {
    roles: $Enums.ROLES;
    sub: number;
}
export interface AuthDTOProps {
    password: string;
    username: string;
}