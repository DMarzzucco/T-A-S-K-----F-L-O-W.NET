using TASK_FLOW.NET.Utils.Filters;

namespace TASK_FLOW.NET.Configuration
{
    public static class ControllerExtension
    {
        public static IServiceCollection AddCustomController(this IServiceCollection service)
        {
            service.AddControllers(e =>
            {
                e.Filters.Add(typeof (GlobalFilterExceptions));
            });
            return service;
        }
    }
}
