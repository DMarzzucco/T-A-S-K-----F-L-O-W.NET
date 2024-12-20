using TASK_FLOW.NET.Utils.Filters;

namespace TASK_FLOW.NET.Configuration
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<GlobalFilterExceptions>();

            return services;
        }
    }
}
