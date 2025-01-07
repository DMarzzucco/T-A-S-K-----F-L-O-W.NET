using TASK_FLOW.NET.User.DTO;

namespace TASK_FLOW.NET.User.Validations.Interface
{
    public interface IUserValidations
    {
        void ValidationCreateUser(CreateUserDTO body);
    }
}
