using Microsoft.AspNetCore.Mvc;

namespace TASK_FLOW.NET.Auth.Helpers
{
    public static class ContentResultHelper
    {
        public static ContentResult CreateContentResult(int statusCodes, string content, string contentType = "application/json")
        {
            return new ContentResult
            {
                Content = content,
                StatusCode = statusCodes,
                ContentType = contentType
            };
        }
    }
}
