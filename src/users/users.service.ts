import { Injectable, NotFoundException, UnauthorizedException } from '@nestjs/common';
import { CreateUserDto } from './dto/create-user.dto';
import { UpdateUserDto } from './dto/update-user.dto';
import { PrismaService } from '../prisma/prisma.service';
import { User } from '@prisma/client';
import * as bcrypt from "bcrypt"
import { findByProps } from './interface/user.interface';

@Injectable()
export class UsersService {

  constructor(private readonly prisma: PrismaService) { }

  async create(body: CreateUserDto): Promise<CreateUserDto> {

    body.password = await bcrypt.hash(body.password, 10)

    return await this.prisma.user.create({ data: body })
  }

  async findAll(): Promise<User[]> {
    return await this.prisma.user.findMany();
  }

  async findOne(id: number): Promise<User> {
    const user = await this.prisma.user.findUnique({ where: { id: id } });
    if (!user) throw new NotFoundException("User not found")
    return user
  }

  async finBy({ key, value }: findByProps): Promise<User> {
    const user: User = await this.prisma.user.findFirst({ where: { [key]: value } })
    if (!user) throw new UnauthorizedException("This username dows not exist in the database")
    return user
  }

  async update(id: number, data: UpdateUserDto): Promise<UpdateUserDto> {
    return await this.prisma.user.update({ where: { id: id }, data: data })
  }

  async updateToken(id: number, refreshToken: string): Promise<User> {
    if (!id) throw new Error("Invalid ID Provided")
    return await this.prisma.user.update({ where: { id }, data: { refreshToken } })
  }

  async remove(id: number): Promise<User> {
    return await this.prisma.user.delete({ where: { id: id } })
  }
}
