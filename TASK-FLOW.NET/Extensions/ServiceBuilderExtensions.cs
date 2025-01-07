using TASK_FLOW.NET.Configuration;
using TASK_FLOW.NET.Configuration.ConnectionsConfigurations;
using TASK_FLOW.NET.Configuration.Swagger;

namespace TASK_FLOW.NET.Extensions
{
    public static class ServiceBuilderExtensions
    {
        public static IServiceCollection AddServicesBuilderExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddDatabaseConfiguration(configuration);
            services.AddHttpContextAccessor();
            //Cors Policy
            services.AddCorsPolicy();
            services.AddJWTAuthentication(configuration);
            //RegisterFilters
            services.AddCustomController();
            //Register Services
            services.AddCustomServices();
            // Swagger Configurations
            services.AddEndpointsApiExplorer();
            services.AddSwaggerConfigurations();
            //Mapper
            services.AddMapperConfig();
            services.AddMvc();
            return services;
        }

    }
}
