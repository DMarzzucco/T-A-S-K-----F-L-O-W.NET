using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.User.Model
{
    public class UserProjectModel
    {
        [SwaggerSchema("UserProject Id")]
        public int Id { get; set; }

        [SwaggerSchema("UserProject Access Level")]
        [SwaggerSchemaExample("OWNER")]
        public required ACCESSLEVEL AccessLevel { get; set; }

        [SwaggerIgnore]
        public int UserId { get; set; }

        [SwaggerSchema("User")]
        [SwaggerSchemaExample("User")]
        public required UsersModel User { get; set; }

        [SwaggerIgnore]
        public int ProjectId { get; set; }

        [SwaggerSchema("Project")]
        [SwaggerSchemaExample("Project")]
        public required ProjectModel Project { get; set; }
    }
}
