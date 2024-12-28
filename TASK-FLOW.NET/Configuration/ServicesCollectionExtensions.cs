using TASK_FLOW.NET.Auth.Cookie.Service;
using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.JWT.Service;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Service;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.Project.Repository;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.Project.Service;
using TASK_FLOW.NET.Project.Service.Interface;
using TASK_FLOW.NET.Tasks.Repository;
using TASK_FLOW.NET.Tasks.Repository.Interface;
using TASK_FLOW.NET.Tasks.Service;
using TASK_FLOW.NET.Tasks.Service.Interface;
using TASK_FLOW.NET.User.Repository;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.User.Service;
using TASK_FLOW.NET.User.Service.Interface;
using TASK_FLOW.NET.UserProject.Repository;
using TASK_FLOW.NET.UserProject.Repository.Interface;
using TASK_FLOW.NET.UserProject.Services;
using TASK_FLOW.NET.UserProject.Services.Interface;
using TASK_FLOW.NET.Utils.Filters;

namespace TASK_FLOW.NET.Configuration
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            //Gloabl Filter Exceptions
            services.AddScoped<GlobalFilterExceptions>();
            //JWT Services
            services.AddScoped<ITokenService, TokenService>();
            // Cookie Service
            services.AddScoped<ICookieService, CookieService>();
            //Jwt Auth Filter 
            services.AddScoped<JwtAuthFilter>();
            // Roles Validations
            services.AddScoped<RolesValidationFilters>();
            //Access Level Validation
            services.AddScoped<AccessLevelAuth>();
            //Auth Services
            services.AddScoped<IAuthService, AuthService>();
            //Local AuthService
            services.AddScoped<LocalAuthFilter>();
            //User Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            //UserProject Services
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();
            services.AddScoped<IUserProjectService, UserProjectService>();
            //Project Services
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();
            //Task Services
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}
