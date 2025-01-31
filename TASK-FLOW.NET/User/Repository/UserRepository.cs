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

        /// <summary>
        ///  Add Change Async 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddChangeAsync(UsersModel user)
        {
            this._context.UserModel.Add(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        ///  Exist By Email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public bool ExistsByEmail(string Email)
        {
            return this._context.UserModel.Any(u => u.Email == Email);
        }

        /// <summary>
        /// Exists By Username 
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public bool ExistsByUsername(string Username)
        {
            return this._context.UserModel.Any(u => u.Username == Username);
        }

        /// <summary>
        /// To List Async 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Find By Id Async 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find By Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<UsersModel?> FindByKey(string key, object value)
        {
            var user = await this._context.UserModel
                .AsQueryable()
                .Where(u => EF.Property<object>(u, key).Equals(value))
                .SingleOrDefaultAsync();
            return user;
        }
        /// <summary>
        /// Remove Async 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task RemoveAsync(UsersModel user)
        {
            this._context.UserModel.Remove(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Async 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
