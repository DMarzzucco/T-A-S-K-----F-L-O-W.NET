namespace TASK_FLOW.NET.Configuration.Swagger.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Struct |
        AttributeTargets.Parameter |
        AttributeTargets.Property |
        AttributeTargets.Enum, AllowMultiple = false)]
    public class SwaggerSchemaExampleAttribute : Attribute
    {
        public SwaggerSchemaExampleAttribute(string exmaple)
        {
            Example = exmaple;
        }
        public string Example { get; set; }
    }
}
