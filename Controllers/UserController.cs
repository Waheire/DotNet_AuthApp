using Auth.Model;
using Auth.Request;
using Auth.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;

        public UserController(IUser user, IMapper mapper)
        {
            _user = user;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<string>> RegisterUser(AddUser addUser) 
        {
            var newUser = _mapper.Map<User>(addUser);

            //password
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            //newUser.Role = "Admin";
            var response = await _user.RegisterUser(newUser);
            return CreatedAtAction(nameof(RegisterUser), response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUser loginUser)
        {
          //check if user with that email exist
          var existingUser = await _user.GetUserByEmail(loginUser.Email);
            if (existingUser == null) 
            {
                return NotFound("Invalid Credentials1");
            }
            //user exists
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginUser.Password, existingUser.Password);
            if (!isPasswordValid) 
            {
                return NotFound("Invalid Credentials2");
            }
            //i provided the right credentials
            //create token
            return Ok($"Welcome {existingUser.Name}");
        }

        private string CreateToken(User user) 
        {
            return "";
        }
    }
}
