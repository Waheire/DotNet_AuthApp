using Auth.Model;
using Auth.Request;
using Auth.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserController(IUser user, IMapper mapper, IConfiguration config)
        {
            _user = user;
            _mapper = mapper;
            _config = config;
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
            var token = CreateToken(existingUser);
            return Ok(token);
        }

        private string CreateToken(User user) 
        {
            //key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("TokenSecuirty:SecretKey")));
            //signing credentials
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Names", user.Name));
            claims.Add(new Claim("Sub", user.Id.ToString()));
            claims.Add(new Claim("Role", user.Role));

            //create Token
            var tokenGenerated = new JwtSecurityToken(
                _config["TokenSecurity:Issuer"],
                _config["TokenSecurity:Audience"],
                 signingCredentials: cred,
                 claims: claims,
                 expires: DateTime.UtcNow.AddHours(1)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            return token;
        }
    }
}
