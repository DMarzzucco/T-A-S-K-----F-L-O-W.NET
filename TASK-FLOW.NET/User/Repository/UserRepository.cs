using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Repository.Interface;

namespace TASK_FLOW.NET.User.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task AddChangeAsync(UsersModel user)
        {
            this._context.UserModel.Add(user);
            await this._context.SaveChangesAsync();
        }

        public bool ExistsByEmail(string Email)
        {
            return this._context.UserModel.Any(u => u.Email == Email);
        }

        public bool ExistsByUsername(string Username)
        {
            return this._context.UserModel.Any(u => u.Username == Username);
        }

        public async Task<UsersModel?> FindAsync()
        {
            return await this._context.UserModel.FindAsync();
        }

        public async Task<UsersModel?> FindByIdAsync(int id)
        {
            return await this._context.UserModel
                .Include(u => u.ProjectIncludes)
                .ThenInclude(pi => pi.Project)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UsersModel?> FindByKey(string key, object value)
        {
            var user = await this._context.UserModel
                .AsQueryable()
                .Where(u => EF.Property<object>(u, key).Equals(value))
                .SingleOrDefaultAsync();
            return user;
        }

        public async Task RemoveAsync(UsersModel user)
        {
            this._context.UserModel.Remove(user);
            await this._context.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsersModel>> ToListAsync()
        {
            return await this._context.UserModel.ToListAsync();
        }

        public async Task UpdateAsync(UsersModel user)
        {
            this._context.UserModel.Entry(user).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }
    }
}
