import { Reflector } from '@nestjs/core';
import { JwtAuthGuard } from './jwt-auth.guard';

describe('JwtAuthGuard', () => {
  let jwtAuthGuard: JwtAuthGuard;
  let reflector:Partial<Reflector>

  beforeEach(() =>{
    reflector = {get:jest.fn()} as Partial<Reflector>
    jwtAuthGuard = new JwtAuthGuard( reflector as Reflector)
  })
  it('should be defined', () => {
    expect(jwtAuthGuard).toBeDefined();
  });
});
