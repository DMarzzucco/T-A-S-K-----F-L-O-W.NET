namespace TASK_FLOW.NET.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousAccessAttribute : Attribute
    {
        public AllowAnonymousAccessAttribute()
        {
            
        }
    }
}
