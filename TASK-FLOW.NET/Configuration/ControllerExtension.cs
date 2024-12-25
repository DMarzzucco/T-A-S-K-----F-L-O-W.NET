using TASK_FLOW.NET.Utils.Filters;
using System.Text.Json.Serialization;

namespace TASK_FLOW.NET.Configuration
{
    public static class ControllerExtension
    {
        public static IServiceCollection AddCustomController(this IServiceCollection service)
        {
            service.AddControllers(static e =>
            {
                e.Filters.Add(typeof(GlobalFilterExceptions));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });
            return service;
        }
    }
}
