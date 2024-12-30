
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.UserProject.Enums;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW_TESTING.UserProject.Mocks;

namespace TASK_FLOW_TESTING.User.Mocks
{
    public static class UsersMock
    {
        public static UsersModel UserMock => new UsersModel
        {
            Id = 4,
            First_name = "Dario",
            Last_name = "Marzzucco",
            Age = "27",
            Username = "DMarzz",
            Email = "DMarzz@gmail.com",
            Password = "promotheus98",
            Roles = ROLES.ADMIN,
            //ProjectIncludes = new List<UserProjectModel> { UserProjectsMocks.MockUserProject }
        };
        public static UsersModel UserMockWithOutProject => new UsersModel
        {
            Id = 5,
            First_name = "Dario",
            Last_name = "Marzzucco",
            Age = "27",
            Username = "DMarzz",
            Email = "DMarzz@gmail.com",
            Password = "promotheus98",
            Roles = ROLES.ADMIN,
        };
        public static UsersModel UserHashPassMock => new UsersModel
        {
            Id = 4,
            First_name = "Dario",
            Last_name = "Marzzucco",
            Age = "27",
            Username = "DMarzz",
            Email = "DMarzz@gmail.com",
            Password = "AQAAAAIAAYagAAAAEEM5GX6vpkzonyKHRs+sSAa22CshBphstUvLsiBfzwYEDjPr5K9WM4xdXbDM1/v6vw==",
            Roles = ROLES.ADMIN
        };

        public static CreateUserDTO CreateUserDTOMOck => new CreateUserDTO
        {
            First_name = "Dario",
            Last_name = "Marzzucco",
            Age = "27",
            Username = "DMarzz",
            Email = "DMarzz@gmail.com",
            Password = "promotheus98",
            Roles = ROLES.ADMIN
        };
        public static UpdateUserDTO UpdateUserDTOMOck => new UpdateUserDTO
        {
            First_name = "Dario",
            Last_name = "Marzzucco",
            Age = "27",
            Username = "DMarzz",
            Email = "DMarzz@gmail.com",
            Password = "promotheus98",
            Roles = ROLES.ADMIN
        };
        public static UsersModel UserMockWithRelations
        {
            get
            {
                var user = new UsersModel
                {
                    Id = 4,
                    First_name = "Dario",
                    Last_name = "Marzzucco",
                    Age = "27",
                    Username = "DMarzz",
                    Email = "DMarzz@gmail.com",
                    Password = "promotheus98",
                    Roles = ROLES.BASIC,
                    ProjectIncludes = new List<UserProjectModel>()
                };

                var project = new ProjectModel
                {
                    Id = 1,
                    Name = "Test Project",
                    Description = "Mocked Project",
                    UsersIncludes = new List<UserProjectModel>()
                };

                var userProject = new UserProjectModel
                {
                    UserId = user.Id,
                    ProjectId = project.Id,
                    AccessLevel = ACCESSLEVEL.DEVELOPER,
                    User = user,        
                    Project = project   
                };

                user.ProjectIncludes.Add(userProject);
                project.UsersIncludes.Add(userProject);

                return user;
            }
        }

    }
}
