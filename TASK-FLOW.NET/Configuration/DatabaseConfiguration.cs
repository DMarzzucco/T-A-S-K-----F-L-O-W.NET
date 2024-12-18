using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;

namespace TASK_FLOW.NET.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            services.AddDbContext<AppDBContext>(op => op.UseNpgsql(connectionString));

            return services;
        }
    }
}
