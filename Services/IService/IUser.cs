using Auth.Model;

namespace Auth.Services.IService
{
    public interface IUser
    {
        Task<string> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
    }
}
