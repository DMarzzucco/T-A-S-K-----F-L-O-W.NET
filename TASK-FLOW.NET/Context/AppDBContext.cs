using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context.Configuration;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW.NET.Context
{
#pragma warning disable CS1591
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<UsersModel> UserModel { get; set; }
        public DbSet<UserProjectModel> UserProjectModel { get; set; }
        public DbSet<ProjectModel> ProjectModel { get; set; }
        public DbSet<TaskModel> TaskModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserModelConfig());
            modelBuilder.ApplyConfiguration(new UserProjectModelConfig());
            modelBuilder.ApplyConfiguration(new ProjectModelConfig());
            modelBuilder.ApplyConfiguration(new TaskModelConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
#pragma warning restore CS1591
}
