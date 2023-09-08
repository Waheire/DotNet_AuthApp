using Auth.Data;
using Auth.Model;
using Auth.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public class UserService : IUser
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<string> RegisterUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "User Registered Successfully";
        }
    }
}
