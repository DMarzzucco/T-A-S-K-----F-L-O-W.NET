using System.Text.RegularExpressions;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.User.Validations.Interface;
using TASK_FLOW.NET.Utils.Exceptions;

namespace TASK_FLOW.NET.User.Validations
{
    public class UsersValidations : IUserValidations
    {
        private readonly IUserRepository _repository;
        public UsersValidations(IUserRepository repository)
        {
            this._repository = repository;
        }
        public void ValidationCreateUser(CreateUserDTO body)
        {

            if (this._repository.ExistsByUsername(body.Username)) throw new ConflictException("This Username already exists");

            if (this._repository.ExistsByEmail(body.Email)) throw new ConflictException("This Email already exists");

            if (body.Password.Length < 8 ||
                !body.Password.Any(char.IsUpper) ||
                !body.Password.Any(char.IsDigit) ||
                !body.Password.Any(ch => !char.IsLetterOrDigit(ch))
                )
                throw new BadRequestException("Password must be at least 8 characters long and contain an uppercase letter, a number, and a special character.");

            if (!Regex.IsMatch(body.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new BadRequestException("Invalid Email format");
        }
    }
}
