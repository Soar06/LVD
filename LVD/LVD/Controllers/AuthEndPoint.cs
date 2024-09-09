using LVD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LVD.Controllers
{
    [Route("auth/[controller]")]
    [ApiController]
    public class AuthEndPoint : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly GeneralFunction _generalFunction;

        // Constructor with dependency injection
        public AuthEndPoint(ChatDbContext context, GeneralFunction generalFunction)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _generalFunction = generalFunction ?? throw new ArgumentNullException(nameof(generalFunction));
        }

        // Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (registerDto == null || string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
            {
                return BadRequest("Invalid registration details provided.");
            }

            // Check if the user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerDto.Username);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            // Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create a new user with the hashed password
            var user = new User
            {
                Username = registerDto.Username,
                Password = passwordHash,
                // Initialize other properties as needed
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Invalid login details provided.");
            }

            // Find the user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Verify the password
            var passwordVerified = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            if (!passwordVerified)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate the JWT token
            var token = _generalFunction.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
