using AutoMapper;
using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //User Mapper
            CreateMap<CreateUserDTO, UsersModel>();
            CreateMap<UpdateUserDTO, UsersModel>();
            // User Project Mapper
            CreateMap<UserProjectDTO, UserProjectModel>();
            //Project Mapper
            CreateMap<CreateProjectDTO, ProjectModel>();
            CreateMap<UpdateProjectDTO, ProjectModel>();

        }
    }
}
