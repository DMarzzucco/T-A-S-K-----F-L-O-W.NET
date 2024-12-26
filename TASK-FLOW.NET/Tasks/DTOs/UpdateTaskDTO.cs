using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.Tasks.Enums;

namespace TASK_FLOW.NET.Tasks.DTOs
{
    public class UpdateTaskDTO
    {
        [SwaggerSchema("Task Name")]
        [SwaggerSchemaExample("Change of management policies")]
        public string? Name { get; set; }

        [SwaggerSchema("Task Desciption")]
        [SwaggerSchemaExample("The changes in management policies have to be different from those of last year.")]
        public string? Descritpion { get; set; }

        [SwaggerSchema("Task Status")]
        [SwaggerSchemaExample("CREATED")]
        public STATUSTASK? Status { get; set; }

        [SwaggerSchema("Responsible Name of the Task")]
        [SwaggerSchemaExample("Dario Marzzucco")]
        public string? ResponsibleName { get; set; }
    }
}
