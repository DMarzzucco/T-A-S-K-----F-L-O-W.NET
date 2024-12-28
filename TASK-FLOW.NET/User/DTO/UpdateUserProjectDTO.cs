using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.User.DTO
{
    public class UpdateUserProjectDTO
    {
        [SwaggerSchema("Access Level")]
        [SwaggerSchemaExample("OWNER")]
        public ACCESSLEVEL? AccessLevel { get; set; }

        [SwaggerSchema("User Id")]
        [SwaggerSchemaExample("UserId:1")]
        public int? UserId { get; set; }

        [SwaggerSchema("Project Id")]
        [SwaggerSchemaExample("ProjectId:1")]
        public int? ProjectId { get; set; }
    }
}
