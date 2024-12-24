using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RolesAttribute:Attribute
    {
        public ROLES[] Roles { get; set; }
        public RolesAttribute(params ROLES[] roles) 
        {
            this.Roles = roles;
        }
    }
}
