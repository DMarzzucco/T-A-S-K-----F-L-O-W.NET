import { Module } from '@nestjs/common';
import { AuthService } from './services/auth.service';

@Module({
  providers: [AuthService]
})
export class AuthModule {}
