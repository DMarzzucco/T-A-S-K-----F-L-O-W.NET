import { ApiProperty } from "@nestjs/swagger";
import { AuthDTOProps } from "../interface/auth.interface";

export class AuthDTO implements AuthDTOProps {
    @ApiProperty({ description: "username", example: "darks" })
    username: string;

    @ApiProperty({ description: "password", example: "949393" })
    password: string;
}