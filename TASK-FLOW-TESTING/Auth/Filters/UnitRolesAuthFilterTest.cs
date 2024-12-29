using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Filters;

namespace TASK_FLOW_TESTING.Auth.Filters
{
    public class UnitRolesAuthFilterTest
    {
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<ITokenService> _tokenService;
        private readonly JwtAuthFilter _filter;

        public UnitRolesAuthFilterTest()
        {
            this._authService = new Mock<IAuthService>();
            this._tokenService = new Mock<ITokenService>();
            this._filter = new JwtAuthFilter(this._authService.Object, this._tokenService.Object);
        }
        [Fact]
        public async Task OnAuthorizationShouldSetUserContextWhenTokenIsValid() { }

        //private static AuthorizationFilterContext CreateAuthorizationFilterContext()
        //{
        //    var context = new DefaultHttpContext();
        //    var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();
        //    var filterContext = new AuthorizationFilterContext(actionDescriptor, new List<IFilterMetadata>());

        //    filterContext.HttpContext = context;
        //    return filterContext;
        //}
    }
}
