using TASK_FLOW.NET.UserProject.DTO;
using TASK_FLOW.NET.UserProject.Enums;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW_TESTING.Project.Mock;
using TASK_FLOW_TESTING.User.Mocks;

namespace TASK_FLOW_TESTING.UserProject.Mocks
{
    public static class UserProjectsMocks
    {
        public static UserProjectModel MockUserProject => new UserProjectModel
        {
            Id = 1,
            AccessLevel = ACCESSLEVEL.OWNER,
            Project = ProjectMock.ProjectModelMock,
            User = UsersMock.UserMock
        };

        public static UserProjectDTO CreateUserProjectDTOMOck => new UserProjectDTO
        {
            AccessLevel = ACCESSLEVEL.OWNER,
            ProjectId = 4,
            UserId = 1
        };
        public static UpdateUserProjectDTO UpdateUserProjectDTOMOck => new UpdateUserProjectDTO
        {
            AccessLevel = ACCESSLEVEL.OWNER,
            ProjectId = 4,
            UserId = 1
        };
    }
}
