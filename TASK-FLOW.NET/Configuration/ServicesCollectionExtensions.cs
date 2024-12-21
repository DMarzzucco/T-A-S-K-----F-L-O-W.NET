using TASK_FLOW.NET.User.Repository;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.User.Service;
using TASK_FLOW.NET.User.Service.Interface;
using TASK_FLOW.NET.Utils.Filters;

namespace TASK_FLOW.NET.Configuration
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            //Gloabl Filter Exceptions
            services.AddScoped<GlobalFilterExceptions>();
            //User Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            //UserProject Services
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();

            return services;
        }
    }
}
