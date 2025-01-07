using Swashbuckle.AspNetCore.Annotations;
using TASK_FLOW.NET.Configuration.Swagger.Attributes;
using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.User.DTO
{
    public class UpdateUserDTO
    {
        [SwaggerSchema("User first name ")]
        [SwaggerSchemaExample("Dario")]
        public string? First_name { get; set; }

        [SwaggerSchema("User last name ")]
        [SwaggerSchemaExample("Marzzucco")]
        public string? Last_name { get; set; }

        [SwaggerSchema("User age ")]
        [SwaggerSchemaExample("27")]
        public string? Age { get; set; }

        [SwaggerSchema("User username ")]
        [SwaggerSchemaExample("DMarzz")]
        public string? Username { get; set; }

        [SwaggerSchema("User Email ")]
        [SwaggerSchemaExample("DMarzz@gmail.com")]
        public string? Email { get; set; }

        [SwaggerSchema("User Password ")]
        [SwaggerSchemaExample("Pr@motheus98")]
        public string? Password { get; set; }

        [SwaggerSchema("User ROle ")]
        [SwaggerSchemaExample("ADMIN")]
        public ROLES? Roles { get; set; }
    }
}
