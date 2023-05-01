using EmployeeAPI.Data;
using EmployeeAPI.DTO;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public UserController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        private async Task<bool> UserExist(string username)
        {
            return await _context.user.AnyAsync(x => x.UserName == username.ToLower());
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.UserName)) return BadRequest("Username is already is use.");

            using var hmac = new HMACSHA512();

            var ur = new User
            {
                FullName = registerDto.FirstName + " " + registerDto.LastName,
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.user.Add(ur);
            await _context.SaveChangesAsync();

            return new User
            {
                UserName = ur.UserName
            };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var u = await _context.user.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (u == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(u.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != u.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Token = _tokenService.CreateToken(u)
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.user.Include(x => x.Posts).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var u = await _context.user.FindAsync(id);

            if (u == null)
            {
                return NotFound();
            }

            return u;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User u)
        {
            var ur = await _context.user.FindAsync(id);

            if (ur != null)
            {
                if(u.FullName != null) ur.FullName = u.FullName; else ur.FullName = ur.FullName;
                if (u.UserName != null) ur.UserName = u.UserName; else ur.UserName = ur.UserName;
                if (u.UserType != null) ur.UserType = u.UserType; else ur.UserType = ur.UserType;
                if (u.Mobile != null) ur.Mobile = u.Mobile; else ur.Mobile = ur.Mobile;
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var u = await _context.user.FindAsync(id);

            if (u == null)
            {
                return NotFound();
            }

            _context.user.Remove(u);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
