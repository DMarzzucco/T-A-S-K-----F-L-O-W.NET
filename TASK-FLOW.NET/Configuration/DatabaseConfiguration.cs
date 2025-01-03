using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.Utils.Helpers;

namespace TASK_FLOW.NET.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");

            DatabaseHelper.WaitForDatabaseAsync(connectionString).GetAwaiter().GetResult();

            services.AddDbContext<AppDBContext>(op => op.UseNpgsql(connectionString));

            return services;
        }
    }
}
