
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;

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
            Roles = ROLES.ADMIN
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
    }
}
