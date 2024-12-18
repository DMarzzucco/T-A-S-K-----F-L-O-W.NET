using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TASK_FLOW.NET.Configuration
{
    public static class SwaggerConfigurations
    {
        public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
        {
            services.AddSwaggerGen(op =>
            {
                op.EnableAnnotations();
                op.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "T A S K - F L O W",
                    Description = "Some description to refomrs"
                });
                //add filter to implement atributes examples in the models and dtos
                var xmLfilesName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                op.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmLfilesName));
            });
            return services;
        }
    }
}
