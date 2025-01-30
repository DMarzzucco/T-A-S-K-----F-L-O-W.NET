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

        public async Task<bool> AddChangeAsync(UserProjectModel body)
        {
            var project = await this._context.ProjectModel.FindAsync(body.ProjectId);

            var user = await this._context.UserModel.FindAsync(body.UserId);

            if (user == null || project == null) return false;

            var entityExisting = this._context.UserProjectModel.Local.FirstOrDefault(u => u.Id == body.Id);

            if (entityExisting != null)
                this._context.Entry(entityExisting).State = EntityState.Detached;

            this._context.Attach(project);
            this._context.Attach(user);

            _context.UserProjectModel.Add(body);
            await _context.SaveChangesAsync();

            return true;
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

        public async Task<bool> UpdateUPAsync(UserProjectModel body)
        {
            _context.UserProjectModel.Entry(body).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
