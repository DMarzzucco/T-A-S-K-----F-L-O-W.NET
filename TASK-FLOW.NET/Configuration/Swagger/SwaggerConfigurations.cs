using Microsoft.OpenApi.Models;
using System.Reflection;
using TASK_FLOW.NET.Configuration.Swagger.Filter;

namespace TASK_FLOW.NET.Configuration.Swagger
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
                op.SchemaFilter<SwaggerSchemaExampleFilter>();

                var xmLfilesName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                op.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmLfilesName));
            });
            return services;
        }
    }
}
