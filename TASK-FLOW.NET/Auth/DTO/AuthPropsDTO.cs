using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;

namespace TASK_FLOW.NET.Auth.DTO
{
    public class AuthPropsDTO
    {
        [SwaggerSchema ("User Username")]
        [SwaggerSchemaExample ("DMarzz")]
        public required string Username { get; set; }

        [SwaggerSchema("User Password ")]
        [SwaggerSchemaExample("promotheus98")]
        public required string Password { get; set; }
    }
}
