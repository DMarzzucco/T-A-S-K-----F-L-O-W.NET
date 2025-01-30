using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.User.Repository.Interface
{
    public interface IUserRepository
    {
        Task SaveChangeAsync();
        Task<UsersModel?> FindByIdAsync(int id);
        Task<IEnumerable<UsersModel>> ToListAsync();
        bool ExistsByEmail(string Email);
        bool ExistsByUsername(string Username);
        Task<UsersModel?> FindAsync();
        Task RemoveAsync(UsersModel user);
        Task AddChangeAsync(UsersModel user);
        Task<bool>UpdateAsync(UsersModel user);
        Task<UsersModel?> FindByKey(string key, object value);
    }
}
