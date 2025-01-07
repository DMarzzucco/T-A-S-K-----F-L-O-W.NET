namespace TASK_FLOW.NET.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseApllicationBuilderExtension(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
