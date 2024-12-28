using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AccessLevelAttribute : Attribute
    {
        public ACCESSLEVEL[] AccessLevel { get; set; }
        public AccessLevelAttribute(params ACCESSLEVEL[] accessLevel)
        {
            this.AccessLevel = accessLevel;
        }
    }
}
