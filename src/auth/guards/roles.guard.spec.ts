import { Reflector } from '@nestjs/core';
import { RolesGuard } from './roles.guard';

describe('RolesGuard', () => {
  let rolesGuard:RolesGuard
  let reflector: Partial<Reflector>

  beforeEach(async() => {
    reflector = {get: jest.fn()}
    rolesGuard = new RolesGuard( reflector as Reflector)
  })
  it('should be defined', () => {
    expect(rolesGuard).toBeDefined();
  });
});
