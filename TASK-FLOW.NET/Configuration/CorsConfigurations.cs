namespace TASK_FLOW.NET.Configuration
{
    public static class CorsConfigurations
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy", b =>
                {
                    b.WithOrigins("http://localhost:300.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            return services;
        }
    }
}
