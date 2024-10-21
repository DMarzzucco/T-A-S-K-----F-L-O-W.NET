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
export interface AuthTokenResult {
    role: string;
    sub: string;
    iat: number;
    exp: number;
}
export interface IUseToken extends Omit<AuthTokenResult, "ia" | "exp"> { isExpire: boolean }