using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Repository.Interface;

namespace TASK_FLOW.NET.UserProject.Repository
{
    public class UserProjectRepository : IUserProjectRepository
    {

        private readonly AppDBContext _context;
        public UserProjectRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddChangeAsync(UserProjectModel body)
        {
            _context.UserProjectModel.Add(body);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProjectModel?> findById(int id)
        {
            var up = await _context.UserProjectModel.FindAsync(id);
            return up;
        }

        public async Task<IEnumerable<UserProjectModel>> ListofAllAsync()
        {
            return await _context.UserProjectModel.ToListAsync();
        }

        public async Task UpdateUPAsync(UserProjectModel body)
        {
            _context.UserProjectModel.Entry(body).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
