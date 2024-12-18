using Microsoft.EntityFrameworkCore;

namespace TASK_FLOW.NET.Context
{
#pragma warning disable CS1591
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
#pragma warning restore CS1591
}
