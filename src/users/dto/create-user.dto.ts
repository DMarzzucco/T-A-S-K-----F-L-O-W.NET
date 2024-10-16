import { $Enums } from "@prisma/client";
import { UserCreate } from "../interface/user.interface";
import { ApiProperty } from "@nestjs/swagger";
// import { ROLES } from "src/constants/roles.constants";

export class CreateUserDto implements UserCreate {
    @ApiProperty({ description: "username", example: "darks" })
    username: string;

    @ApiProperty({ description: "password", example: "949393" })
    password: string;

    @ApiProperty({ description: "roles", example: "ADMIN" })
    roles: $Enums.ROLES;
}
