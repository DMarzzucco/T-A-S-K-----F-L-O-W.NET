using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW_TESTING.Auth.Helper;
using Moq;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;

namespace TASK_FLOW_TESTING.Auth.Attributes
{
    public class UnitAllowAnonymousAccessTest
    {
        private readonly Mock<IAuthService> _authSerivce;
        private readonly Mock<ITokenService> _tokenService;
        private readonly JwtAuthFilter _jwtFilter;
        private readonly AccessLevelAuth _accessLevelFilter;

        public UnitAllowAnonymousAccessTest()
        {
            this._authSerivce = new Mock<IAuthService>();
            this._tokenService = new Mock<ITokenService>();

            this._jwtFilter = new JwtAuthFilter(
                this._authSerivce.Object,
                this._tokenService.Object
                );
            this._accessLevelFilter = new AccessLevelAuth(this._authSerivce.Object);
        }

        [Fact]
        public async Task ShouldAllowPublicAccess_WhenAllowAnonymousAccessAttributeIsPresent()
        {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            context.ActionDescriptor.EndpointMetadata = new List<object> { new AllowAnonymousAccessAttribute() };

            var RolesFilter = new RolesValidationFilters();

            await this._jwtFilter.OnAuthorizationAsync(context);
            await this._accessLevelFilter.OnAuthorizationAsync(context);

            RolesFilter.OnAuthorization(context);

            Assert.Null(context.Result);
        }
    }
}
