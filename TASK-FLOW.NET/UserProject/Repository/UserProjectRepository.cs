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
        /// <summary>
        /// Add Change Async 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Fin by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserProjectModel?> findById(int id)
        {
            var up = await _context.UserProjectModel.FindAsync(id);
            return up;
        }
        /// <summary>
        /// List Of All Relations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserProjectModel>> ListofAllAsync()
        {
            return await _context.UserProjectModel.ToListAsync();
        }
        /// <summary>
        /// Update Relations
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUPAsync(UserProjectModel body)
        {
            _context.UserProjectModel.Entry(body).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
