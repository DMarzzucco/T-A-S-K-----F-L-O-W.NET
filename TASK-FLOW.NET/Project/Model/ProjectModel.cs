using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.Task.Model;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Project.Model
{
    public class ProjectModel
    {
        [SwaggerSchema("Project id")]
        public int Id { get; set; }

        [SwaggerSchema("Project Name")]
        [SwaggerSchemaExample("Next year's projects.")]
        public required string Name { get; set; }

        [SwaggerSchema("Project Description")]
        [SwaggerSchemaExample("Next year's projects include basic tasks for beginners, as well as a review of errors made the previous year.")]
        public required string Description { get; set; }

        [SwaggerIgnore]
        public ICollection<UserProjectModel> UsersIncludes { get; set; } = new List<UserProjectModel>();

        [SwaggerIgnore]
        public ICollection<TaskModel> Task { get; set; } = new List<TaskModel>();
    }
}
