using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.UserProject.Enums;

namespace TASK_FLOW.NET.UserProject.DTO
{
    public class UpdateUserProjectDTO
    {
        [SwaggerSchema("Access Level")]
        [SwaggerSchemaExample("DEVELOPER")]
        public ACCESSLEVEL? AccessLevel { get; set; }

        [SwaggerSchema("User Id")]
        [SwaggerSchemaExample("1")]
        public int? UserId { get; set; }

        [SwaggerSchema("Project Id")]
        [SwaggerSchemaExample("1")]
        public int? ProjectId { get; set; }
    }
}
