using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.UserProject.Model;

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
        public async Task<IEnumerable<UsersModel>> ToListAsync()
        {
            return await this._context.UserModel.Select(u => new UsersModel
            {
                Id = u.Id,
                First_name = u.First_name,
                Last_name = u.Last_name,
                Age = u.Age,
                Username = u.Username,
                Email = u.Email,
                Password = u.Password,
                Roles = u.Roles,
                RefreshToken = u.RefreshToken,
                ProjectIncludes = new List<UserProjectModel>()
            }).ToListAsync();
        }

        public async Task<UsersModel?> FindByIdAsync(int id)
        {
            var user = await this._context.UserModel
                .Include(u => u.ProjectIncludes)
                .ThenInclude(pi => pi.Project)
                .ThenInclude(pi=> pi.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            user.ProjectIncludes ??= new List<UserProjectModel>();
            return user;
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


        public async Task<bool> UpdateAsync(UsersModel user)
        {
            var date = await this._context.UserModel.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (date == null) return false;

            var existingUser = this._context.UserModel.Local.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser != null)
            {
                this._context.Entry(existingUser).State = EntityState.Detached;
            }

            this._context.Attach(user);

            this._context.UserModel.Entry(user).State = EntityState.Modified;
            await this._context.SaveChangesAsync();

            return true;
        }
    }
}
