using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Repository.Interface;

namespace TASK_FLOW.NET.User.Repository
{
    public class UserProjectRepository : IUserProjectRepository
    {

        private readonly AppDBContext _context;
        public UserProjectRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task AddChangeAsync(UserProjectModel body)
        {
            this._context.UserProjectModel.Add(body);
            await this._context.SaveChangesAsync();
        }

        public async Task<UserProjectModel?> findById(int id)
        {
            var up = await this._context.UserProjectModel.FindAsync(id);
            return up;
        }

        public async Task<IEnumerable<UserProjectModel>> ListofAllAsync()
        {
            return await this._context.UserProjectModel.ToListAsync();
        }

        public async Task UpdateUPAsync(UserProjectModel body)
        {
            this._context.UserProjectModel.Entry(body).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }
    }
}
