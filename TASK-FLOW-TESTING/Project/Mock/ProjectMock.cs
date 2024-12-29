
using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;

namespace TASK_FLOW_TESTING.Project.Mock
{
    public static class ProjectMock
    {
        public static ProjectModel ProjectModelMock => new ProjectModel
        {
            Id = 4,
            Name = "Project test",
            Description = "This is a project test"
        };
        public static CreateProjectDTO CreateProjectDTOMock => new CreateProjectDTO
        {
            Name = "Project test",
            Description = "This is a project test"
        };
        public static UpdateProjectDTO UpdateProjectDTOMock => new UpdateProjectDTO
        {
            Name = "Project test",
            Description = "This is a project test"
        };
    }
}
