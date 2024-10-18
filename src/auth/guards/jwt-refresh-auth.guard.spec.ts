import { JwtRefreshAuthGuard } from './jwt-refresh-auth.guard';

describe('JwtRefreshAuthGuard', () => {
  it('should be defined', () => {
    expect(new JwtRefreshAuthGuard()).toBeDefined();
  });
});
