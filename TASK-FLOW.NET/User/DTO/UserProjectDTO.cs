using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.User.DTO
{
    public class UserProjectDTO
    {
        [SwaggerSchema("Access Level")]
        [SwaggerSchemaExample("OWNER")]
        public required ACCESSLEVEL AccessLevel { get; set; }

        [SwaggerSchema("User")]
        public required UsersModel User { get; set; }

        [SwaggerSchema("Project")]
        public required ProjectModel Project { get; set; }

    }
}
