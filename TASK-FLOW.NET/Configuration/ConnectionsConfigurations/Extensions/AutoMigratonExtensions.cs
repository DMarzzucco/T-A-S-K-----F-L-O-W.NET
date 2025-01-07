using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;

namespace TASK_FLOW.NET.Configuration.ConnectionsConfigurations.Extensions
{
    public static class AutoMigratonExtensions
    {
        public static void ApplyMigration(this IHost app) {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            dbContext.Database.Migrate();
        }
    }
}
