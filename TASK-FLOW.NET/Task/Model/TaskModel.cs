using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Task.Enums;

namespace TASK_FLOW.NET.Task.Model
{
    public class TaskModel
    {
        [SwaggerSchema("Task id")]
        public int Id { get; set; }

        [SwaggerSchema("Task Name")]
        [SwaggerSchemaExample("Change of management policies")]
        public required string Name { get; set; }

        [SwaggerSchema("Task Desciption")]
        [SwaggerSchemaExample("The changes in management policies have to be different from those of last year.")]
        public required string Descritpion { get; set; }

        [SwaggerSchema("Task Status")]
        [SwaggerSchemaExample("CREATED")]
        public required STATUSTASK Status { get; set; }

        [SwaggerSchema("Responsible Name of the Task")]
        [SwaggerSchemaExample("Dario Marzzucco")]
        public required string ResponsibleName { get; set; }

        [SwaggerIgnore]
        public int ProjectId { get; set; }

        [SwaggerSchema("Project")]
        [SwaggerSchemaExample("Project")]
        public required ProjectModel Project { get; set; }
    }
}
