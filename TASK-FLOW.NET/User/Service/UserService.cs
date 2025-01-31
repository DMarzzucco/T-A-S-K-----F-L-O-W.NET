using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.User.Service.Interface;
using TASK_FLOW.NET.User.Validations.Interface;

namespace TASK_FLOW.NET.User.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly IUserValidations _validation;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper, IUserValidations validations)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._validation = validations;
        }
        /// <summary>
        /// Save one Register
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<UsersModel> CreateUser(CreateUserDTO body)
        {
            this._validation.ValidationCreateUser(body);

            var data = this._mapper.Map<UsersModel>(body);

            var passwordHasher = new PasswordHasher<UsersModel>();
            data.Password = passwordHasher.HashPassword(data, body.Password);

            await this._repository.AddChangeAsync(data);
            return data;
        }
        /// <summary>
        /// Delete One register
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task DeleteUser(int id)
        {
            var user = await this._repository.FindByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            await this._repository.RemoveAsync(user);
        }

        /// <summary>
        /// Fin by value key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UsersModel> FindByAuth(string key, object value)
        {
            var user = await this._repository.FindByKey(key, value);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
        /// <summary>
        /// Get all Users Register
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UsersModel>> GetAll()
        {
            return await this._repository.ToListAsync();
        }

        /// <summary>
        /// Get One Register
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UsersModel> GetById(int id)
        {
            var user = await this._repository.FindByIdAsync(id);
            if (user == null) throw new KeyNotFoundException(" User not found");
            return user;
        }

        /// <summary>
        /// Update RefrehsToken
        /// </summary>
        /// <param name="id"></param>
        /// <param name="RefreshToken"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UsersModel> UpdateToken(int id, string RefreshToken)
        {
            var user = await this._repository.FindByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("Invalid Id Provider");

            user.RefreshToken = RefreshToken;

            await this._repository.UpdateAsync(user);
            return user;
        }

        /// <summary>
        /// Update one Register
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UsersModel> UpdateUser(int id, UpdateUserDTO body)
        {
            var user = await this._repository.FindByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            this._mapper.Map(body, user);

            var passwordHasher = new PasswordHasher<UsersModel>();
            user.Password = passwordHasher.HashPassword(user, body.Password);

            await this._repository.UpdateAsync(user);

            return user;
        }
    }
}
